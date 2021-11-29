using System.Collections.Generic;
using UnityEditor;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Effects/UI_Sort", 14)]
    public class UISort : BaseMeshEffect
    {
        [SerializeField]
        private int orderIndex = 0;

        protected UISort()
        { }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
        }

#endif

        public int OrderIndex
        {
            get { return orderIndex; }
            set
            {
                orderIndex = value;
                if (graphic != null)
                    graphic.SetVerticesDirty();
            }
        }

        public override void ModifyMesh(VertexHelper vh)
        {
            if (!IsActive())
                return;

            List<UIVertex> output = new List<UIVertex>();
            vh.GetUIVertexStream(output);

            transform.SetSiblingIndex(orderIndex);

            vh.Clear();
            vh.AddUIVertexTriangleStream(output);
        }
    }
}