namespace Assets.Kanau.ThreeScene {
    public interface IThreeElem
    {
        string Uuid { get; }
        void Accept(IVisitor v);
    }
}
