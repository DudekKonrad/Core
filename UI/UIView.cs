using System.Collections.Generic;
using Application.Utils.UIExtensions;
using UnityEngine;
using UnityEngine.UIElements;
namespace Application.Core.UI
{
    [RequireComponent(typeof(UIDocument))]
    public abstract class UIView : MonoBehaviour
    {
        private UIDocument _document;
        private VisualElement _root;

        private readonly Dictionary<string, VisualElement> _elementCache = new();

        protected virtual void Awake()
        {
            _document = GetComponent<UIDocument>();
            _root = _document.rootVisualElement;
        }

        protected virtual void OnEnable()
        {
            Initialize();
        }

        protected abstract void Initialize();

        protected T Get<T>(string name) where T : VisualElement
        {
            if (_elementCache.TryGetValue(name, out var cachedElement))
            {
                return cachedElement as T;
            }

            var element = _root.Q<T>(name);
            if (element == null)
            {
                Debug.LogError($"[UI] Not found UI element: {name} in object: {gameObject.name}");
                return null;
            }

            _elementCache[name] = element;
            return element;
        }

        public virtual void Show() => _root.Show();
        public virtual void Hide() => _root.Hide();
    }
}