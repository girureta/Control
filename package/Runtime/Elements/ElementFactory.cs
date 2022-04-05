using System;
using System.Collections.Generic;
using System.Linq;

namespace Control
{
    public class ElementFactory : IElementFactory
    {
        private readonly Dictionary<Type, Type> typeDictionary = new Dictionary<Type, Type>();
        private Type initialWebElementType;

        public IElement CreateWebElement<K>(K sourceObject)
        {
            var sourceType = sourceObject.GetType();

            Type webElementType = null;

            if (!typeDictionary.ContainsKey(sourceType))
            {
                webElementType = typeDictionary.FirstOrDefault(x => x.Key.IsAssignableFrom(sourceType)).Value;
            }
            else
            {
                webElementType = typeDictionary[sourceType];
            }

            if (webElementType == null)
                return null;

            IElement instance = (IElement)Activator.CreateInstance(webElementType, sourceObject);
            return instance;
        }

        public void SetInitialWebElementType<T>() where T : IElement
        {
            initialWebElementType = typeof(T);
        }

        public IElement GetRootWebElement()
        {
            if (initialWebElementType == null)
                return null;

            IElement instance = (IElement)Activator.CreateInstance(initialWebElementType);
            return instance;
        }

        public void RegisterWebElementType<T, K>() where T : Element<K>
        {
            var webElementType = typeof(T);
            var sourceType = typeof(K);
            typeDictionary.Add(sourceType, webElementType);
        }
    }
}