using Assets.Kanau.ThreeScene.Materials;

namespace Assets.Kanau.AFrameScene.Materials {
    public interface IMaterialFactory {
        AFrameMaterial Create(MaterialElem elem);
    }
}
