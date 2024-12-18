using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NiceRectUI
{
    public enum FillType
    {
        SolidColor,
        Gradient
    }

    public enum GradientType
    {
        Linear,
        Radial
    }

    public enum StrokeType
    {
        NoStroke,
        InnerStroke,
    }

    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(RectTransform), typeof(Graphic))]
    public class NiceRect : UIBehaviour
    {
        private static readonly int Props_WidthHeight = Shader.PropertyToID("_WidthHeightSize");
        private static readonly int Props_Corner_Radius = Shader.PropertyToID("_Corner");
        private static readonly int Props_Top = Shader.PropertyToID("_Top");
        private static readonly int Props_Bottom = Shader.PropertyToID("_Bottom");
        private static readonly int Props_Grayscale = Shader.PropertyToID("_UseGrayscale");
        private static readonly int Props_GrayscaleFactor = Shader.PropertyToID("_GrayscaleFactor");
        private static readonly int Props_FillType = Shader.PropertyToID("_FillType");
        private static readonly int Props_FillColor = Shader.PropertyToID("_FillColor");
        private static readonly int Props_GradientColorLength = Shader.PropertyToID("_GradientColorLength");
        private static readonly int Props_GradientAlphaLength = Shader.PropertyToID("_GradientAlphaLength");
        private static readonly int Props_GradientRotation = Shader.PropertyToID("_GradientRotation");
        private static readonly int Props_GradientType = Shader.PropertyToID("_GradientType");
        private static readonly int Props_GradientScale = Shader.PropertyToID("_GradientScale");
        private static readonly int Props_StrokeType = Shader.PropertyToID("_StrokeType");
        private static readonly int Props_StrokeColorType = Shader.PropertyToID("_StrokeColorType");
        private static readonly int Props_StrokeWidth = Shader.PropertyToID("_StrokeWidth");
        private static readonly int Props_StrokeColor = Shader.PropertyToID("_StrokeColor");
        private static readonly int Props_StrokeGradientColorLength = Shader.PropertyToID("_StrokeGradientColorLength");
        private static readonly int Props_StrokeGradientAlphaLength = Shader.PropertyToID("_StrokeGradientAlphaLength");
        private static readonly int Props_StrokeGradientType = Shader.PropertyToID("_StrokeGradientType");
        private static readonly int Props_StrokeGradientScale = Shader.PropertyToID("_StrokeGradientScale");
        private static readonly int Props_StrokeGradientRotation = Shader.PropertyToID("_StrokeGradientRotation");

        private const float Epsilon = 0.001f;

        public Vector2 _topLeft = Vector2.zero;
        public Vector2 _topRight = Vector2.zero;
        public Vector2 _botLeft = Vector2.zero;
        public Vector2 _botRight = Vector2.zero;
        public float _cornerRadius = 0;
        public FillOption _fillOption = new FillOption();
        public bool _useGrayScale = false;
        [Range(-0.5f, 0.5f)]
        public float _grayScaleFactor = 0f;
        public StrokeType _strokeType = StrokeType.NoStroke;
        public float _strokeWidth = 1.0f;
        public FillOption _strokeOption = new FillOption() { color = Color.black };

        public Action OnMaterialBlockChanged;

        private Material _material;
        private Material _overrideMaterial;
        private MaterialPropertyBlock _maskMaterialPropertyBlock;

        [HideInInspector, SerializeField] private MaskableGraphic image;

        [SerializeField] [HideInInspector]
        private Rect _rect;
        private bool _isDirty = false;

        public Gradient FillGradient
        {
            get => _fillOption.gradient;
            set
            {
                _fillOption.gradient = value;
                SetDirty();
            }
        }

        public Gradient StrokeGradient
        {
            get => _strokeOption.gradient;
            set
            {
                _strokeOption.gradient = value;
                SetDirty();
            }
        }

        public Color FillColor
        {
            get => _fillOption.color;
            set
            {
                _fillOption.color = value;
                SetDirty();
            }
        }

        public Color StrokeColor
        {
            get => _strokeOption.color;
            set
            {
                _strokeOption.color = value;
                SetDirty();
            }
        }

        public FillType FillType
        {
            get => _fillOption.fillType;
            set
            {
                _fillOption.fillType = value;
                SetDirty();
            }
        }

        public StrokeType StrokeType
        {
            get => _strokeType;
            set
            {
                _strokeType = value;
                SetDirty();
            }
        }

        public float FillGradientRotation
        {
            get => _fillOption.gradientRotation;
            set
            {
                _fillOption.gradientRotation = value;
                SetDirty();
            }
        }

        public FillType StrokeFillType
        {
            get => _strokeOption.fillType;
            set
            {
                _strokeOption.fillType = value;
                SetDirty();
            }
        }

        protected override void OnRectTransformDimensionsChange()
        {
            if (enabled && _material != null)
            {
                UpdateUIRect();
                ValidateComponent();
                Refresh(_material);
                if (_overrideMaterial != _material)
                    Refresh(_overrideMaterial);
                Refresh(_maskMaterialPropertyBlock);
            }
        }

        private void OnValidate()
        {
            ValidateComponent();
            Refresh(_material);
            if (_overrideMaterial != _material)
                Refresh(_overrideMaterial);
            Refresh(_maskMaterialPropertyBlock);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)transform);
        }

        private void OnEnable()
        {
            UpdateUIRect();
            ValidateComponent();
            Refresh(_material);
            if (_overrideMaterial != _material)
                Refresh(_overrideMaterial);
            Refresh(_maskMaterialPropertyBlock);
        }

        private void OnDestroy()
        {
            if (image != null)
                image.material = null;

            DestroyHelper.Destroy(_material);
            image = null;
            _material = null;
            _maskMaterialPropertyBlock = null;
        }

        private void ValidateComponent()
        {
            if (_material == null)
                _material = new Material(Shader.Find("Custom/SlantedUIShader"));


            if (image == null)
                TryGetComponent(out image);

            if (image != null)
                image.material = _material;

            _overrideMaterial = image.materialForRendering;

            if (_maskMaterialPropertyBlock == null)
                _maskMaterialPropertyBlock = new MaterialPropertyBlock();
        }

        public void ValidateData()
        {
            _topLeft = ValidateSize(_topLeft, _botLeft, _topRight, _rect);
            _topRight = ValidateSize(_topRight, _botRight, _topLeft, _rect);
            _botLeft = ValidateSize(_botLeft, _topLeft, _botRight, _rect);
            _botRight = ValidateSize(_botRight, _topRight, _botLeft, _rect);
            _topLeft = ValidateZero(_topLeft);
            _topRight = ValidateZero(_topRight);
            _botLeft = ValidateZero(_botLeft);
            _botRight = ValidateZero(_botRight);
            _cornerRadius = ValidateCorner(_cornerRadius, _rect);
        }

        private Vector2 ValidateSize(Vector2 pos, Vector2 vPos, Vector2 hPos, Rect dimension)
        {
            pos.x = (pos.x + hPos.x >= dimension.width)
                    ? (dimension.width - Mathf.Min(hPos.x, dimension.width - Epsilon) - Epsilon)
                    : Mathf.Min(pos.x, dimension.width - Epsilon);
            pos.y = (pos.y + vPos.y >= dimension.height)
                    ? (dimension.height - Mathf.Min(vPos.y, dimension.height - Epsilon) - Epsilon)
                    : Mathf.Min(pos.y, dimension.height - Epsilon);
            return pos;
        }

        private Vector2 ValidateZero(Vector2 a)
        {
            return Vector2.Max(Vector2.zero, a);
        }

        private float ValidateCorner(float corner, Rect rect)
        {
            var maxSize = Mathf.Min(rect.width - Mathf.Max(_topLeft.x + _topRight.x, _botLeft.x + _botRight.x),
                    rect.height - Mathf.Max(_topLeft.y + _botLeft.y, _topRight.y + _botRight.y)) / 2 - Epsilon;
            return Mathf.Clamp(corner, 0, maxSize);
        }

        private void Refresh(Material material)
        {
            material.SetVector(Props_WidthHeight, new Vector4(_rect.width, _rect.height, 0, 0));
            material.SetFloat(Props_Corner_Radius, _cornerRadius);
            material.SetVector(Props_Top, new Vector4(_topLeft.x, _topLeft.y, _topRight.x, _topRight.y));
            material.SetVector(Props_Bottom, new Vector4(_botLeft.x, _botLeft.y, _botRight.x, _botRight.y));
            material.SetInteger(Props_Grayscale, _useGrayScale ? 1 : 0);
            material.SetFloat(Props_GrayscaleFactor, _grayScaleFactor);
            material.SetInteger(Props_FillType, (int)_fillOption.fillType);
            material.SetColor(Props_FillColor, _fillOption.color);
            material.SetInteger(Props_StrokeType, (int)_strokeType);
            material.SetInteger(Props_StrokeColorType, (int)_strokeOption.fillType);
            material.SetFloat(Props_StrokeWidth, _strokeWidth);
            material.SetColor(Props_StrokeColor, _strokeOption.color);
            RefreshGradient(material);
            image.SetVerticesDirty();
        }

        private void Refresh(MaterialPropertyBlock material)
        {
            material.SetVector(Props_WidthHeight, new Vector4(_rect.width, _rect.height, 0, 0));
            material.SetFloat(Props_Corner_Radius, _cornerRadius);
            material.SetVector(Props_Top, new Vector4(_topLeft.x, _topLeft.y, _topRight.x, _topRight.y));
            material.SetVector(Props_Bottom, new Vector4(_botLeft.x, _botLeft.y, _botRight.x, _botRight.y));
            material.SetInteger(Props_Grayscale, _useGrayScale ? 1 : 0);
            material.SetFloat(Props_GrayscaleFactor, _grayScaleFactor);
            material.SetInteger(Props_FillType, (int)_fillOption.fillType);
            material.SetColor(Props_FillColor, _fillOption.color);
            material.SetInteger(Props_StrokeType, (int)_strokeType);
            material.SetInteger(Props_StrokeColorType, (int)_strokeOption.fillType);
            material.SetFloat(Props_StrokeWidth, _strokeWidth);
            material.SetColor(Props_StrokeColor, _strokeOption.color);
            RefreshGradient(material);
            OnMaterialBlockChanged?.Invoke();
        }


        private void RefreshGradient(Material material)
        {
            if (_fillOption.fillType == FillType.Gradient)
            {
                var gradientColors = _fillOption.gradient.colorKeys.Select(it =>
                {
                    Color c = it.color;
                    c.a = it.time;
                    return c;
                }).ToArray();
                var gradientAlphas = _fillOption.gradient.alphaKeys.Select(it =>
                {
                    Color c = default;
                    c.r = it.alpha;
                    c.g = it.time;
                    return c;
                }).ToArray();

                for (int i = 0; i < gradientColors.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_GradientColors_{i}"), gradientColors[i]);
                }
                for (int i = 0; i < gradientAlphas.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_GradientAlphas_{i}"), gradientAlphas[i]);
                }
                material.SetInteger(Props_GradientColorLength, gradientColors.Length);
                material.SetInteger(Props_GradientAlphaLength, gradientAlphas.Length);
                material.SetFloat(Props_GradientRotation, _fillOption.gradientRotation);
                material.SetFloat(Props_GradientScale, _fillOption.gradientScale);
                material.SetInteger(Props_GradientType, (int)_fillOption.gradientType);
            }

            if (_strokeOption.fillType == FillType.Gradient)
            {
                var gradientColors = _strokeOption.gradient.colorKeys.Select(it =>
                {
                    Color c = it.color;
                    c.a = it.time;
                    return c;
                }).ToArray();
                var gradientAlphas = _strokeOption.gradient.alphaKeys.Select(it =>
                {
                    Color c = default;
                    c.r = it.alpha;
                    c.g = it.time;
                    return c;
                }).ToArray();

                for (int i = 0; i < gradientColors.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_StrokeGradientColors_{i}"), gradientColors[i]);
                }
                for (int i = 0; i < gradientAlphas.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_StrokeGradientAlphas_{i}"), gradientAlphas[i]);
                }
                material.SetInteger(Props_StrokeGradientColorLength, gradientColors.Length);
                material.SetInteger(Props_StrokeGradientAlphaLength, gradientAlphas.Length);
                material.SetFloat(Props_StrokeGradientRotation, _strokeOption.gradientRotation);
                material.SetFloat(Props_StrokeGradientScale, _strokeOption.gradientScale);
                material.SetInteger(Props_StrokeGradientType, (int)_strokeOption.gradientType);
            }

        }

        private void RefreshGradient(MaterialPropertyBlock material)
        {
            if (_fillOption.fillType == FillType.Gradient)
            {
                var gradientColors = _fillOption.gradient.colorKeys.Select(it =>
                {
                    Color c = it.color;
                    c.a = it.time;
                    return c;
                }).ToArray();
                var gradientAlphas = _fillOption.gradient.alphaKeys.Select(it =>
                {
                    Color c = default;
                    c.r = it.alpha;
                    c.g = it.time;
                    return c;
                }).ToArray();

                for (int i = 0; i < gradientColors.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_GradientColors_{i}"), gradientColors[i]);
                }
                for (int i = 0; i < gradientAlphas.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_GradientAlphas_{i}"), gradientAlphas[i]);
                }
                material.SetInteger(Props_GradientColorLength, gradientColors.Length);
                material.SetInteger(Props_GradientAlphaLength, gradientAlphas.Length);
                material.SetFloat(Props_GradientRotation, _strokeOption.gradientRotation);
            }

            if (_strokeOption.fillType == FillType.Gradient)
            {
                var gradientColors = _fillOption.gradient.colorKeys.Select(it =>
                {
                    Color c = it.color;
                    c.a = it.time;
                    return c;
                }).ToArray();
                var gradientAlphas = _fillOption.gradient.alphaKeys.Select(it =>
                {
                    Color c = default;
                    c.r = it.alpha;
                    c.g = it.time;
                    return c;
                }).ToArray();

                for (int i = 0; i < gradientColors.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_StrokeGradientColors_{i}"), gradientColors[i]);
                }
                for (int i = 0; i < gradientAlphas.Length; i++)
                {
                    material.SetColor(Shader.PropertyToID($"_StrokeGradientAlphas_{i}"), gradientAlphas[i]);
                }
                material.SetInteger(Props_StrokeGradientColorLength, gradientColors.Length);
                material.SetInteger(Props_StrokeGradientAlphaLength, gradientAlphas.Length);
                material.SetFloat(Props_StrokeGradientRotation, _fillOption.gradientRotation);
            }
        }

        private void UpdateUIRect()
        {
            _rect = ((RectTransform)transform).rect;
        }

        public MaterialPropertyBlock GetMaterialPropertyBlock()
        {
            return _maskMaterialPropertyBlock;
        }

        private void Update()
        {
            if (_isDirty)
            {
                ValidateComponent();
                Refresh(_material);
                if (_overrideMaterial != _material)
                    Refresh(_overrideMaterial);
                Refresh(_maskMaterialPropertyBlock);
                _isDirty = false;
            }
        }

        public void SetDirty()
        {
            _isDirty = true;
        }
    }

    internal static class DestroyHelper
    {
        internal static void Destroy(UnityEngine.Object @object)
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                UnityEngine.Object.Destroy(@object);
            }
            else
            {
                UnityEngine.Object.DestroyImmediate(@object);
            }
#else
            UnityEngine.Object.Destroy(@object);
#endif
        }
    }
}