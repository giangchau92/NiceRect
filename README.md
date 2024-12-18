# Unity Nice Rect 

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