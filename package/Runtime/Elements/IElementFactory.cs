namespace Control
{
    public interface IElementFactory
    {
        IElement GetRootWebElement();
        void RegisterWebElementType<T, K>() where T : Element<K>;
        IElement CreateWebElement<K>(K sourceObject);
    }
}