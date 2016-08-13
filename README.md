# UnitySceneWebExporter
Export Three.js or A-Frame From Unity Scene

## Screenshot
### Unity3D scene
![Unity3D scene](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/SimpleViewer/documents/sample-scene-unity.png)

### Three.js
![Three.js](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/SimpleViewer/documents/sample-scene-threejs.png)

### A-Frame
![A-Frame](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/SimpleViewer/documents/sample-scene-aframe.png)

## Feature
* Export Scene to Three.js scene format (THREE.ObjectLoader can load exported data)
* Export Scene to A-Frame document
* Export C# Script variable
* Support lightmapping

## How to use
1. Make scene in Unity3D (tested on 5.4.0f3)
2. Edit->Kanau->"Export AFrame" or "Export Three.js"
3. Export

## Note
* Export configurations are used in A-Frame exporting. Three.js doesn't use it.
* A-Frame is unstable library. (current A-Frame version is 0.2.0) In future, A-Frame exporter wiil be broken.

## Similar Project / Library
* [J3D - unity3d-to-threejs exporter](https://github.com/drojdjou/J3D)
* [UnityAFrameExporter - Export A-Frame From Unity Scene.](https://github.com/umiyuki/UnityAFrameExporter)
* [Three.js JSON Exporter - Asset Store](https://www.assetstore.unity3d.com/en/#!/content/40550)
