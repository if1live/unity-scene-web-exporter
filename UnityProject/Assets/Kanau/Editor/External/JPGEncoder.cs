using UnityEngine;
using System.Collections;
using System.IO;
using System.Threading;


/**
 * Class that converts BitmapData into a valid JPEG
 */		
public class JPGEncoder
{

	/**
	* This is an AS3 class--just emulated the parts the encoder uses
	*/
	private class ByteArray
	{
		private MemoryStream stream;
		private BinaryWriter writer;
		
		public ByteArray()
		{
			stream = new MemoryStream();
			writer = new BinaryWriter(stream);
		}
		
		/**
		* Function from AS3--add a byte to our stream
		*/
		public void WriteByte( byte value  )
		{
			writer.Write(value);
		}
		
		/**
		* Spit back all bytes--to either pass via WWW or save to disk
		*/
		public byte[] GetAllBytes()
		{
			byte[] buffer = new byte[stream.Length];
			stream.Position = 0;
			stream.Read(buffer, 0, buffer.Length);
			
			return buffer;
		}
	}
	
	private struct BitString 
	{
		public int length;
		public int value;
	}
	
	/**
	* Another flash class--emulating the stuff the encoder uses
	*/
	private class BitmapData
	{
		public int height;
		public int width;
		
		private Color32[] pixels;
		
		/**
		* Pull all of our pixels off the texture (Unity stuff isn't thread safe, and this is faster)
		*/
		public BitmapData ( Texture2D texture )
		{
			this.height = texture.height;
			this.width = texture.width;
			
			pixels = texture.GetPixels32();
		}
	
		/**
		* Mimic the flash function
		*/
		public Color32 GetPixelColor( int x, int y )
		{	
			x = Mathf.Clamp(x, 0, width - 1);
			y = Mathf.Clamp(y, 0, height - 1);
	
			return pixels[y * width + x];
		}
	}
		
	// Static table initialization
	private int[] ZigZag = new int[]{
		 0, 1, 5, 6,14,15,27,28,
		 2, 4, 7,13,16,26,29,42,
		 3, 8,12,17,25,30,41,43,
		 9,11,18,24,31,40,44,53,
		10,19,23,32,39,45,52,54,
		20,22,33,38,46,51,55,60,
		21,34,37,47,50,56,59,61,
		35,36,48,49,57,58,62,63
	};

	private int[] YTable = new int[64];
	private int[] UVTable = new int[64];
	private float[] fdtbl_Y = new float[64];
	private float[] fdtbl_UV = new float[64];

	private void InitQuantTables ( int sf  ){
		int i;
		float t;
		int[] YQT = new int[]{
			16, 11, 10, 16, 24, 40, 51, 61,
			12, 12, 14, 19, 26, 58, 60, 55,
			14, 13, 16, 24, 40, 57, 69, 56,
			14, 17, 22, 29, 51, 87, 80, 62,
			18, 22, 37, 56, 68,109,103, 77,
			24, 35, 55, 64, 81,104,113, 92,
			49, 64, 78, 87,103,121,120,101,
			72, 92, 95, 98,112,100,103, 99
		};
		
		for (i = 0; i < 64; i++) 
		{
			t = Mathf.Floor((YQT[i]*sf+50)/100);
			t = Mathf.Clamp(t, 1, 255);
			YTable[ZigZag[i]] = Mathf.RoundToInt( t );
		}
		
		int[] UVQT = new int[]{
			17, 18, 24, 47, 99, 99, 99, 99,
			18, 21, 26, 66, 99, 99, 99, 99,
			24, 26, 56, 99, 99, 99, 99, 99,
			47, 66, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99,
			99, 99, 99, 99, 99, 99, 99, 99
		};
		for (i = 0; i < 64; i++) 
		{
			t = Mathf.Floor((UVQT[i] * sf + 50) / 100);
			t = Mathf.Clamp(t, 1, 255);
			UVTable[ZigZag[i]] = (int) t; 
		}
		
		
		float[] aasf = new float[]
		{
			1.0f, 1.387039845f, 1.306562965f, 1.175875602f,
			1.0f, 0.785694958f, 0.541196100f, 0.275899379f
		};
		
		i = 0;
		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 8; col++)
			{
				fdtbl_Y[i]  = (1.0f /  (YTable [ZigZag[i]] * aasf[row] * aasf[col] * 8.0f));
				fdtbl_UV[i] = (1.0f /  (UVTable[ZigZag[i]] * aasf[row] * aasf[col] * 8.0f));
				i++;
			}
		}
	}

	private BitString[] YDC_HT;
	private BitString[] UVDC_HT;
	private BitString[] YAC_HT;
	private BitString[] UVAC_HT;
	private BitString[] ComputeHuffmanTbl ( byte[] nrcodes , byte[] std_table )
	{
		int codevalue = 0;
		int pos_in_table = 0;
		BitString[] HT = new BitString[16 * 16];
		for (int k=1; k<=16; k++) 
		{
			for (int j=1; j<=nrcodes[k]; j++) 
			{
				HT[std_table[pos_in_table]] = new BitString();
				HT[std_table[pos_in_table]].value = codevalue;
				HT[std_table[pos_in_table]].length = k;
				pos_in_table++;
				codevalue++;
			}
			codevalue*=2;
		}
		return HT;
	}

	private byte[] std_dc_luminance_nrcodes = new byte[]{0,0,1,5,1,1,1,1,1,1,0,0,0,0,0,0,0};
	private byte[] std_dc_luminance_values = new byte[]{0,1,2,3,4,5,6,7,8,9,10,11};
	private byte[] std_ac_luminance_nrcodes = new byte[]{0,0,2,1,3,3,2,4,3,5,5,4,4,0,0,1,0x7d};
	private byte[] std_ac_luminance_values = new byte[]{
		0x01,0x02,0x03,0x00,0x04,0x11,0x05,0x12,
		0x21,0x31,0x41,0x06,0x13,0x51,0x61,0x07,
		0x22,0x71,0x14,0x32,0x81,0x91,0xa1,0x08,
		0x23,0x42,0xb1,0xc1,0x15,0x52,0xd1,0xf0,
		0x24,0x33,0x62,0x72,0x82,0x09,0x0a,0x16,
		0x17,0x18,0x19,0x1a,0x25,0x26,0x27,0x28,
		0x29,0x2a,0x34,0x35,0x36,0x37,0x38,0x39,
		0x3a,0x43,0x44,0x45,0x46,0x47,0x48,0x49,
		0x4a,0x53,0x54,0x55,0x56,0x57,0x58,0x59,
		0x5a,0x63,0x64,0x65,0x66,0x67,0x68,0x69,
		0x6a,0x73,0x74,0x75,0x76,0x77,0x78,0x79,
		0x7a,0x83,0x84,0x85,0x86,0x87,0x88,0x89,
		0x8a,0x92,0x93,0x94,0x95,0x96,0x97,0x98,
		0x99,0x9a,0xa2,0xa3,0xa4,0xa5,0xa6,0xa7,
		0xa8,0xa9,0xaa,0xb2,0xb3,0xb4,0xb5,0xb6,
		0xb7,0xb8,0xb9,0xba,0xc2,0xc3,0xc4,0xc5,
		0xc6,0xc7,0xc8,0xc9,0xca,0xd2,0xd3,0xd4,
		0xd5,0xd6,0xd7,0xd8,0xd9,0xda,0xe1,0xe2,
		0xe3,0xe4,0xe5,0xe6,0xe7,0xe8,0xe9,0xea,
		0xf1,0xf2,0xf3,0xf4,0xf5,0xf6,0xf7,0xf8,
		0xf9,0xfa
	};

	private byte[] std_dc_chrominance_nrcodes = new byte[]{0,0,3,1,1,1,1,1,1,1,1,1,0,0,0,0,0};
	private byte[] std_dc_chrominance_values = new byte[]{0,1,2,3,4,5,6,7,8,9,10,11};
	private byte[] std_ac_chrominance_nrcodes = new byte[]{0,0,2,1,2,4,4,3,4,7,5,4,4,0,1,2,0x77};
	private byte[] std_ac_chrominance_values = new byte[]{
		0x00,0x01,0x02,0x03,0x11,0x04,0x05,0x21,
		0x31,0x06,0x12,0x41,0x51,0x07,0x61,0x71,
		0x13,0x22,0x32,0x81,0x08,0x14,0x42,0x91,
		0xa1,0xb1,0xc1,0x09,0x23,0x33,0x52,0xf0,
		0x15,0x62,0x72,0xd1,0x0a,0x16,0x24,0x34,
		0xe1,0x25,0xf1,0x17,0x18,0x19,0x1a,0x26,
		0x27,0x28,0x29,0x2a,0x35,0x36,0x37,0x38,
		0x39,0x3a,0x43,0x44,0x45,0x46,0x47,0x48,
		0x49,0x4a,0x53,0x54,0x55,0x56,0x57,0x58,
		0x59,0x5a,0x63,0x64,0x65,0x66,0x67,0x68,
		0x69,0x6a,0x73,0x74,0x75,0x76,0x77,0x78,
		0x79,0x7a,0x82,0x83,0x84,0x85,0x86,0x87,
		0x88,0x89,0x8a,0x92,0x93,0x94,0x95,0x96,
		0x97,0x98,0x99,0x9a,0xa2,0xa3,0xa4,0xa5,
		0xa6,0xa7,0xa8,0xa9,0xaa,0xb2,0xb3,0xb4,
		0xb5,0xb6,0xb7,0xb8,0xb9,0xba,0xc2,0xc3,
		0xc4,0xc5,0xc6,0xc7,0xc8,0xc9,0xca,0xd2,
		0xd3,0xd4,0xd5,0xd6,0xd7,0xd8,0xd9,0xda,
		0xe2,0xe3,0xe4,0xe5,0xe6,0xe7,0xe8,0xe9,
		0xea,0xf2,0xf3,0xf4,0xf5,0xf6,0xf7,0xf8,
		0xf9,0xfa
	};

	private void InitHuffmanTbl ()
	{
		YDC_HT = ComputeHuffmanTbl(std_dc_luminance_nrcodes,std_dc_luminance_values);
		UVDC_HT = ComputeHuffmanTbl(std_dc_chrominance_nrcodes,std_dc_chrominance_values);
		YAC_HT = ComputeHuffmanTbl(std_ac_luminance_nrcodes,std_ac_luminance_values);
		UVAC_HT = ComputeHuffmanTbl(std_ac_chrominance_nrcodes,std_ac_chrominance_values);
	}

	private BitString[] bitcode = new BitString[65535];
	private int[] category = new int[65535];
	
	private void InitCategoryfloat ()
	{
		int nrlower = 1;
		int nrupper = 2;
		int nr;
		BitString bs;
		for (int cat=1; cat<=15; cat++) 
		{
			//Positive numbers
			for (nr=nrlower; nr<nrupper; nr++) 
			{
				category[32767+nr] = cat;

				bs = new BitString();
				bs.length = cat;
				bs.value = nr;
				bitcode[32767+nr] = bs;
			}
			//Negative numbers
			for (nr=-(nrupper-1); nr<=-nrlower; nr++) 
			{
				category[32767+nr] = cat;

				bs = new BitString();
				bs.length = cat;
				bs.value = nrupper-1+nr;
				bitcode[32767+nr] = bs;
			}
			nrlower <<= 1;
			nrupper <<= 1;
		}
	}

	// IO functions
	private uint bytenew = 0;
	private int bytepos = 7;
	private ByteArray byteout = new ByteArray();
	
	/**
	* Get the result
	*/
	public byte[] GetBytes (){
		if(!isDone)
		{
			Debug.LogError("JPEGEncoder not complete, cannot get bytes!");
			return null;
		}
		
		return byteout.GetAllBytes();
	}
	
	private void WriteBits ( BitString bs  ){
		int value = bs.value;
		int posval = bs.length-1;
		while ( posval >= 0 ) 
		{
			if ( (value & System.Convert.ToUInt32(1 << posval)) != 0 )
			{
				bytenew |= System.Convert.ToUInt32(1 << bytepos);
			}
			posval--;
			bytepos--;
			if (bytepos < 0) 
			{
				if (bytenew == 0xFF) 
				{
					WriteByte(0xFF);
					WriteByte(0);
				}
				else
				{
					WriteByte((byte)bytenew);
				}
				bytepos=7;
				bytenew=0;
			}
		}
	}

	private void WriteByte ( byte value  )
	{
		byteout.WriteByte(value);
	}

	private void WriteWord ( int value )
	{
		WriteByte((byte) (( value >> 8)	&0xFF) );
		WriteByte((byte) (( value )		&0xFF) );
	}

	// DCT & quantization core
	private float[] FDCTQuant ( float[] data ,   float[] fdtbl  )
	{
		float tmp0; float tmp1; float tmp2; float tmp3; float tmp4;
		float tmp5; float tmp6; float tmp7;
		float tmp10; float tmp11; float tmp12; float tmp13;
		float z1; float z2; float z3; float z4; float z5; float z11; float z13;
		int i;
		/* Pass 1: process rows. */
		int dataOff=0;
		for (i=0; i<8; i++) {
			tmp0 = data[dataOff+0] + data[dataOff+7];
			tmp7 = data[dataOff+0] - data[dataOff+7];
			tmp1 = data[dataOff+1] + data[dataOff+6];
			tmp6 = data[dataOff+1] - data[dataOff+6];
			tmp2 = data[dataOff+2] + data[dataOff+5];
			tmp5 = data[dataOff+2] - data[dataOff+5];
			tmp3 = data[dataOff+3] + data[dataOff+4];
			tmp4 = data[dataOff+3] - data[dataOff+4];

			/* Even part */
			tmp10 = tmp0 + tmp3;	/* phase 2 */
			tmp13 = tmp0 - tmp3;
			tmp11 = tmp1 + tmp2;
			tmp12 = tmp1 - tmp2;

			data[dataOff+0] = tmp10 + tmp11; /* phase 3 */
			data[dataOff+4] = tmp10 - tmp11;

			z1 = (tmp12 + tmp13) * 0.707106781f; /* c4 */
			data[dataOff+2] = tmp13 + z1; /* phase 5 */
			data[dataOff+6] = tmp13 - z1;

			/* Odd part */
			tmp10 = tmp4 + tmp5; /* phase 2 */
			tmp11 = tmp5 + tmp6;
			tmp12 = tmp6 + tmp7;

			/* The rotator is modified from fig 4-8 to avoid extra negations. */
			z5 = (tmp10 - tmp12) * 0.382683433f; /* c6 */
			z2 = 0.541196100f * tmp10 + z5; /* c2-c6 */
			z4 = 1.306562965f * tmp12 + z5; /* c2+c6 */
			z3 = tmp11 * 0.707106781f; /* c4 */

			z11 = tmp7 + z3;	/* phase 5 */
			z13 = tmp7 - z3;

			data[dataOff+5] = z13 + z2;	/* phase 6 */
			data[dataOff+3] = z13 - z2;
			data[dataOff+1] = z11 + z4;
			data[dataOff+7] = z11 - z4;

			dataOff += 8; /* advance pointer to next row */
		}

		/* Pass 2: process columns. */
		dataOff = 0;
		for (i=0; i<8; i++) {
			tmp0 = data[dataOff+ 0] + data[dataOff+56];
			tmp7 = data[dataOff+ 0] - data[dataOff+56];
			tmp1 = data[dataOff+ 8] + data[dataOff+48];
			tmp6 = data[dataOff+ 8] - data[dataOff+48];
			tmp2 = data[dataOff+16] + data[dataOff+40];
			tmp5 = data[dataOff+16] - data[dataOff+40];
			tmp3 = data[dataOff+24] + data[dataOff+32];
			tmp4 = data[dataOff+24] - data[dataOff+32];

			/* Even part */
			tmp10 = tmp0 + tmp3;	/* phase 2 */
			tmp13 = tmp0 - tmp3;
			tmp11 = tmp1 + tmp2;
			tmp12 = tmp1 - tmp2;

			data[dataOff+ 0] = tmp10 + tmp11; /* phase 3 */
			data[dataOff+32] = tmp10 - tmp11;

			z1 = (tmp12 + tmp13) * 0.707106781f; /* c4 */
			data[dataOff+16] = tmp13 + z1; /* phase 5 */
			data[dataOff+48] = tmp13 - z1;

			/* Odd part */
			tmp10 = tmp4 + tmp5; /* phase 2 */
			tmp11 = tmp5 + tmp6;
			tmp12 = tmp6 + tmp7;

			/* The rotator is modified from fig 4-8 to avoid extra negations. */
			z5 = (tmp10 - tmp12) * 0.382683433f; /* c6 */
			z2 = 0.541196100f * tmp10 + z5; /* c2-c6 */
			z4 = 1.306562965f * tmp12 + z5; /* c2+c6 */
			z3 = tmp11 * 0.707106781f; /* c4 */

			z11 = tmp7 + z3;	/* phase 5 */
			z13 = tmp7 - z3;

			data[dataOff+40] = z13 + z2; /* phase 6 */
			data[dataOff+24] = z13 - z2;
			data[dataOff+ 8] = z11 + z4;
			data[dataOff+56] = z11 - z4;

			dataOff++; /* advance pointer to next column */
		}

		// Quantize/descale the coefficients
		for (i=0; i<64; i++) {
			// Apply the quantization and scaling factor & Round to nearest integer
			data[i] = Mathf.Round((data[i]*fdtbl[i]));
		}
		return data;
	}

	// Chunk writing

	private void WriteAPP0 ()
	{
		WriteWord(0xFFE0); // marker
		WriteWord(16); // length
		WriteByte(0x4A); // J
		WriteByte(0x46); // F
		WriteByte(0x49); // I
		WriteByte(0x46); // F
		WriteByte(0); // = "JFIF",'\0'
		WriteByte(1); // versionhi
		WriteByte(1); // versionlo
		WriteByte(0); // xyunits
		WriteWord(1); // xdensity
		WriteWord(1); // ydensity
		WriteByte(0); // thumbnwidth
		WriteByte(0); // thumbnheight
	}

	private void WriteSOF0( int width , int height )
	{
		WriteWord(0xFFC0); // marker
		WriteWord(17);   // length, truecolor YUV JPG
		WriteByte(8);    // precision
		WriteWord(height);
		WriteWord(width);
		WriteByte(3);    // nrofcomponents
		WriteByte(1);    // IdY
		WriteByte(0x11); // HVY
		WriteByte(0);    // QTY
		WriteByte(2);    // IdU
		WriteByte(0x11); // HVU
		WriteByte(1);    // QTU
		WriteByte(3);    // IdV
		WriteByte(0x11); // HVV
		WriteByte(1);    // QTV
	}

	private void WriteDQT()
	{
		WriteWord(0xFFDB); // marker
		WriteWord(132);	   // length
		WriteByte(0);
		int i;
		for (i=0; i<64; i++) {
			WriteByte((byte)YTable[i]);
		}
		WriteByte(1);
		for (i=0; i<64; i++) {
			WriteByte((byte)UVTable[i]);
		}
	}

	private void WriteDHT()
	{
		WriteWord(0xFFC4); // marker
		WriteWord(0x01A2); // length
		int i;

		WriteByte(0); // HTYDCinfo
		for (i=0; i<16; i++) {
			WriteByte(std_dc_luminance_nrcodes[i+1]);
		}
		for (i=0; i<=11; i++) {
			WriteByte(std_dc_luminance_values[i]);
		}

		WriteByte(0x10); // HTYACinfo
		for (i=0; i<16; i++) {
			WriteByte(std_ac_luminance_nrcodes[i+1]);
		}
		for (i=0; i<=161; i++) {
			WriteByte(std_ac_luminance_values[i]);
		}

		WriteByte(1); // HTUDCinfo
		for (i=0; i<16; i++) {
			WriteByte(std_dc_chrominance_nrcodes[i+1]);
		}
		for (i=0; i<=11; i++) {
			WriteByte(std_dc_chrominance_values[i]);
		}

		WriteByte(0x11); // HTUACinfo
		for (i=0; i<16; i++) {
			WriteByte(std_ac_chrominance_nrcodes[i+1]);
		}
		for (i=0; i<=161; i++) {
			WriteByte(std_ac_chrominance_values[i]);
		}
	}

	private void writeSOS(){
		WriteWord(0xFFDA); // marker
		WriteWord(12); // length
		WriteByte(3); // nrofcomponents
		WriteByte(1); // IdY
		WriteByte(0); // HTY
		WriteByte(2); // IdU
		WriteByte(0x11); // HTU
		WriteByte(3); // IdV
		WriteByte(0x11); // HTV
		WriteByte(0); // Ss
		WriteByte(0x3f); // Se
		WriteByte(0); // Bf
	}

	// Core processing
	private int[] DU = new int[64];
	private float ProcessDU( float[] CDU , float[] fdtbl , float DC , BitString[] HTDC , BitString[] HTAC )
	{
		BitString EOB = HTAC[0x00];
		BitString M16zeroes = HTAC[0xF0];
		int i;

		float[] DU_DCT = FDCTQuant(CDU, fdtbl);
		//ZigZag reorder
		for (i=0;i<64;i++) 
		{
			DU[ZigZag[i]]= (int) DU_DCT[i];
		}
		int Diff = (int) ( DU[0] - DC ); 
		DC = DU[0];
		
		//Encode DC
		if (Diff==0) 
		{
			WriteBits(HTDC[0]); // Diff might be 0
		} 
		else 
		{
			WriteBits(HTDC[category[32767+Diff]]);
			WriteBits(bitcode[32767+Diff]);
		}
		//Encode ACs
		int end0pos = 63;
		for (; (end0pos>0)&&(DU[end0pos]==0); end0pos--) { };
		
		//end0pos = first element in reverse order !=0
		if ( end0pos == 0) 
		{
			WriteBits(EOB);
			return DC;
		}
		i = 1;
		while ( i <= end0pos )
		{
			int startpos = i;
			for (; (DU[i]==0) && (i<=end0pos); i++) {}
			
			int nrzeroes = i-startpos;
			if ( nrzeroes >= 16 ) 
			{
				for (int nrmarker=1; nrmarker <= nrzeroes/16; nrmarker++) 
				{
					WriteBits(M16zeroes);
				}
				nrzeroes = (nrzeroes&0xF);
			}
			WriteBits(HTAC[nrzeroes*16+category[32767+DU[i]]]);
			WriteBits(bitcode[32767+DU[i]]);
			i++;
		}
		if ( end0pos != 63 ) 
		{
			WriteBits(EOB);
		}
		return DC;
	}

	private float[] YDU = new float[64];
	private float[] UDU = new float[64];
	private float[] VDU = new float[64];
	private void RGB2YUV( BitmapData image, int xpos, int ypos)
	{		
		int pos = 0;
		for (int y = 0; y < 8; y++) 
		{
			for (int x = 0; x < 8; x++) 
			{
				Color32 C = image.GetPixelColor(xpos+x, image.height - (ypos+y));
				YDU[pos]=((( 0.29900f)*C.r +( 0.58700f)*C.g+( 0.11400f)*C.b))-128;
				UDU[pos]=(((-0.16874f)*C.r +(-0.33126f)*C.g+( 0.50000f)*C.b));
				VDU[pos]=((( 0.50000f)*C.r +(-0.41869f)*C.g+(-0.08131f)*C.b));
				pos++;
			}
		}
	}

	/**
	 * Constructor for JPEGEncoder class
	 *
	 * @param quality The quality level between 1 and 100 that detrmines the
	 * level of compression used in the generated JPEG
	 * @langversion ActionScript 3.0f
	 * @playerversion Flash 9.0f
	 * @tiptext
	 */
	// public flag--other scripts must watch this to know when they can safely get data out
	public bool isDone = false;
	private BitmapData image;
	private int sf = 0;
	private string path;
	private int cores = 0;
	//Contructor that doesn't save the data to disk
	public JPGEncoder( Texture2D texture, float quality) : this(texture, quality, "", false){ }
	public JPGEncoder( Texture2D texture, float quality, bool blocking) : this(texture, quality, "", blocking){ }
	public JPGEncoder( Texture2D texture, float quality, string path) : this(texture, quality, path, false){ }
	public JPGEncoder( Texture2D texture, float quality, string path, bool blocking )
	{	
		this.path = path;
		// save out texture data to our own data structure
		image = new BitmapData(texture);
		
		quality = Mathf.Clamp(quality, 1, 100);
		sf = (quality < 50) ? (int)(5000 / quality) : (int)(200 - quality*2);
		
		cores = SystemInfo.processorCount;
		
		Thread thread = new Thread(DoEncoding);
		thread.Start();		
		
		if( blocking )
			thread.Join();
	}
	
	/**
	* Handle initialization, encoding and writing to disk
	*/
	private void DoEncoding()
	{
		isDone = false;
	
		// Create tables -- technically we could only do this once for multiple encodes
		InitHuffmanTbl();
		InitCategoryfloat();
		InitQuantTables(sf);
		
		// Do actual encoding
		Encode();
		
		if( !string.IsNullOrEmpty(path) )
			File.WriteAllBytes(path, GetBytes());
		
		// signal that our data is ok to use now
		isDone = true;
	}
	
	/**
	 * Created a JPEG image from the specified BitmapData
	 *
	 * @param image The BitmapData that will be converted into the JPEG format.
	 * @return a ByteArray representing the JPEG encoded image data.
	 * @langversion ActionScript 3.0f
	 * @playerversion Flash 9.0f
	 * @tiptext
	 */	
	private void Encode()
	{
		// Initialize bit writer
		byteout = new ByteArray();
		bytenew=0;
		bytepos=7;

		// Add JPEG headers
		WriteWord(0xFFD8); // SOI
		WriteAPP0();
		WriteDQT();
		WriteSOF0(image.width,image.height);
		WriteDHT();
		writeSOS();
		
		// Encode 8x8 macroblocks
		float DCY=0;
		float DCU=0;
		float DCV=0;
		bytenew=0;
		bytepos=7;
		for (int ypos = 0; ypos < image.height; ypos += 8 ) 
		{
			for (int xpos = 0; xpos < image.width; xpos += 8) 
			{
				RGB2YUV(image, xpos, ypos);
				DCY = ProcessDU(YDU, fdtbl_Y,  DCY, YDC_HT,  YAC_HT);
				DCU = ProcessDU(UDU, fdtbl_UV, DCU, UVDC_HT, UVAC_HT);
				DCV = ProcessDU(VDU, fdtbl_UV, DCV, UVDC_HT, UVAC_HT);
				
				//If running on a single core system, then give some time to do other stuff
				if( cores == 1 )
					Thread.Sleep(0);
			}
		}

		// Do the bit alignment of the EOI marker
		if ( bytepos >= 0 ) 
		{
			BitString fillbits = new BitString();
			fillbits.length = bytepos+1;
			fillbits.value = (1<<(bytepos+1))-1;
			WriteBits(fillbits);
		}

		WriteWord(0xFFD9); //EOI
		
		//Signal we are done
		isDone = true;
	}
}

/*
* Ported to Unity C# by Andreas Broager
*/

/*
* Ported to UnityScript by Matthew Wegner, Flashbang Studios
* 
* Original code is from as3corelib, found here:
* http://code.google.com/p/as3corelib/source/browse/trunk/src/com/adobe/images/JPGEncoder.as
* 
* Original copyright notice is below:
*/

/*
  Copyright (c) 2008, Adobe Systems Incorporated
  All rights reserved.

  Redistribution and use in source and binary forms, with or without 
  modification, are permitted provided that the following conditions are
  met:

  * Redistributions of source code must retain the above copyright notice, 
    this list of conditions and the following disclaimer.
  
  * Redistributions in binary form must reproduce the above copyright
    notice, this list of conditions and the following disclaimer in the 
    documentation and/or other materials provided with the distribution.
  
  * Neither the name of Adobe Systems Incorporated nor the names of its 
    contributors may be used to endorse or promote products derived from 
    this software without specific prior written permission.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
  IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO,
  THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
  PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR 
  CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
  EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
  PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
  PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
  LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
  NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/

