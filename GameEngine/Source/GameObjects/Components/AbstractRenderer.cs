using GameEngine.GameObjects;
using GameEngine.Resources;

namespace GameEngine.Components
{
    public abstract class AbstractRenderer : Component
    {
        public Material Material { get; private set; }

        protected AbstractRenderer(GameObject gameObject) : base(gameObject)
        {
        }

        public void SetMaterial(Material material)
        {
            if (material == null)
            {
                throw new NullReferenceException("Material can't be null");
            }

            Material = material;
        }
    }
}