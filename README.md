# Nice Rect for UGUI 

## 📝 Description
Unity Nice Rect is a simple UIImage extension that allows you to create a nice looking rectangle in the Unity.

## 📌 Key Features
- Slanted rectangle
- Rounded corner rectangle
- Rectangle with a border
- Solid and gradient filled rectangle
- Solid and gradient stroke border
- Support Unity Mask and UI Soft Mask
- Support Anti-aliasing

## 📦 Installation

### Install via UPM (with Package Manager UI)

- Click `Window > Package Manager` to open Package Manager UI.
- Click `+ > Add package from git URL...` and input the repository URL:
`https://github.com/giangchau92/NiceRect.git`
- 
### Install via UPM (Manually)

- Open the `Packages/manifest.json` file in your project. Then add this package somewhere in the `dependencies` block:
  ```json
  {
    "dependencies": {
      "com.nc981.nicerect": "https://github.com/giangchau92/NiceRect.git",
      ...
    }
  }
  ```

## 🚀 Usage

### Getting Started

1. [Install the package](#-installation).

2. Add a `NiceRect` component in to `UIImage` object.

> Please note that `NiceRect` required `UIImage` component to work.

3. Adjust the `NiceRect` parameters in the inspector.  

4. Enjoy!

### Component NiceRect

**Corner Section**

* Top Left: offset from the top left corner of the rect.
* Top Right: offset from the top right corner of the rect.
* Bottom Left: offset from the bottom left corner of the rect.
* Bottom Right: offset from the bottom right corner of the rect.

**Fill Section**

* FillType: Fill type of the rect
  * Solid Color: Fill the rect with a solid color
  * Gradient: Fill the rect with a gradient color
* Color: color of the rect
* Gradient Type: Gradient type
  * Linear: Linear gradient
  * Radial: Radial gradient
* Gradient: gradient color
* Gradient Scale: stretch the gradient color
* Gradient Rotation: rotation of the gradient

**Stroke Section**

* Stroke Type: Stroke type of the rect
  * No Stroke: no stroke rect
  * Inner Stroke: stroke inside the rect
* Stroke Width: width of the stroke
* Fill Type: fill type of the stroke
  * Solid Color: Fill the stroke with a solid color
  * Gradient: Fill the stroke with a gradient color
* Gradient Type: Gradient type of the stroke
  * Linear: Linear gradient
  * Radial: Radial gradient
* Gradient: gradient color of the stroke
* Gradient Scale: stretch the gradient color of the stroke
* Gradient Rotation: rotation of the gradient color of the stroke

**Gray Scale**
* Use Gray Scale: convert the rect to gray scale
* Gray Scale Factor: strength of the gray scale

## License

* MIT

## Author
* [Giang Chau](https://github.com/giangchau92)