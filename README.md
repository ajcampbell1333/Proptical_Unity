# Proptical Unity Package
**Free Open-Source MOCAP optical+RF Inside-out-OR-Outside-in full-body-OR-prop AI-driven 3D pose tracking**


[![Version](https://img.shields.io/github/v/tag/ajcampbell1333/Proptical_Unity?label=Proptical&color=lightgreen)](https://github.com/ajcampbell1333/Proptical_Unity/releases)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![Unity Package](https://img.shields.io/badge/Unity-2022.3%2B-blue)](https://unity.com)

> The open-source motion capture system that tracks real-time 3D pose for any prop, hand, or body using $12 ESP32-CAMs, Wi-Fi, UWB anchors, and your choice of state-of-the-art video-to-3D-pose deep learning models.

Proptical Unity Package provides VRPN-based motion capture integration for Unity projects.

**Cross-Platform Support:** This package is part of a cross-platform SDK. See also: [Unreal Engine Plugin](../Proptical_Unreal/Plugins/Proptical/README.md)

>**DISCLAIMER:** This plugin is in Pre-Alpha - not ready for primetime by a longshot yet!

---

## 📋 Overview

The Proptical Plugin provides VRPN-based motion capture integration for Unity projects.

It's an open-source motion capture system that tracks real-time 3D pose for any prop, hand, or body using $12 ESP32-CAMs, Wi-Fi, UWB anchors, and your choice of state-of-the-art video-to-3D-pose deep learning models.

Proptical will be the **Blender of mocap** – a single local server that speaks VRPN, OSC, and UDP, works with Unity and Unreal out-of-the-box, and lets artists choose between fiducial markers, markerless bodies, or semantic single-point tracking to drive and export 3D pose in real-time to other 3D applications (or use it directly in game engine).

---

## Table of Contents

<details>
<summary><strong>THIS README</strong></summary>

- [Overview](#-overview)
- [Features](#-features)
- [Installation](#-installation)
- [Quick Start](#-quick-start)
- [Documentation](#-documentation)
- [Requirements](#-requirements)
- [Roadmap](#-roadmap)
- [Support](#-support)
- [License](#-license)
- [Contributing](#-contributing)
- [Credits](#-credits)

</details>

<details>
<summary><strong>OTHER READMES IN THIS PROJECT</strong></summary>

- [Unreal Engine Plugin](../Proptical_Unreal/Plugins/Proptical/README.md)

</details>

---

## ✨ Features

* **VRPN Server Integration** - Drop-in Vicon/OptiTrack replacement using official VRPN library with named rigid bodies and skeletons
* **Native C# Wrappers** - Unity-optimized C# wrappers for the Proptical C++ core
* **Fiducial Tracking** - OpenCV ArUco / ChArUco sub-mm precision tracking
* **Markerless Body Tracking** - Outside-in multi-person tracking via NIM containers (MMPose, MediaPipe, CLIFF, MvP)
* **Markerless Single-Point Tracking** - Inside-out 3D transforms via NIM containers (KP3D, OnePose, NOPE, VideoPose3D)
* **UWB Fusion** - Kalman filter-based 2D world-map fusion with anchor calibration
* **Auto-Calibration** - One-tap ChArUco board wave calibration
* **OSC/UDP Support** - Direct OSC and UDP communication alongside VRPN
* **Unity Demo Scene** - Example scene demonstrating prop tracking with 1:1 cube following

---

## 📦 Installation

### Via Unity Package Manager (Git URL)

1. Open Unity and go to **Window > Package Manager**
2. Click the **+** button and select **Add package from git URL**
3. Enter: `https://github.com/ajcampbell1333/Proptical_Unity.git?path=/Assets/Proptical`
4. Click **Add**

### Via Git Submodule

```bash
cd Assets
git submodule add https://github.com/ajcampbell1333/Proptical_Unity.git Proptical
```

### Manual Installation

1. Download or clone this repository
2. Copy the `Assets/Proptical` folder into your Unity project's `Assets` folder
3. Unity will automatically import the package

---

## 🚀 Quick Start

1. Install the Proptical Server and configure your ESP32-CAMs
2. Start the Proptical Server with your desired tracking mode
3. Import the Proptical Unity Package into your project
4. Add the VRPN Tracker component to your GameObject
5. Configure the tracker to connect to your Proptical Server
6. Your GameObject will now follow the tracked prop/body in real-time

---

## 📚 Documentation

- [Getting Started Guide](docs/GettingStarted.md)
- [API Reference](docs/API.md)
- [Examples](docs/Examples.md)

---

## ⚙️ Requirements

- Unity 2022.3 or later
- Proptical Server running on local network
- ESP32-CAM hardware (for tracking)

---

## 🗺️ Roadmap

<details>
<summary><strong>v0.0.1 – Pre-Alpha ("It tracks one thing really, really well")</strong></summary>

- ✅ Create public GitHub repo + MIT license + initial folder structure
- ✅ Write and freeze the v0.0.1 README
- ✅ Basic package structure and namespace setup
- ✅ VRPN client integration for Unity (UDP-focused, minimal protocol structure)
- ✅ VRPN client integration for Unreal (UDP-focused, minimal protocol structure)
- ✅ Cross-platform Phase 2 implementation (Unity + Unreal)

</details>

<details>
<summary><strong>v0.0.2 – Pre-Alpha (In Progress)</strong></summary>

- 🚧 VRPN Tracker message parsing (requires protocol specification)
- 🚧 VRPNTrackedObject (base demo MonoBehaviour for Unity)
- 🚧 VRPNTransformNode (transform node with primitive visualizers - Cube, Sphere, Cylinder)
- 🚧 Unity demo scene creation with multiple transform nodes
- 🚧 Testing and validation of core tracking accuracy
- 🚧 CI – GitHub Actions build and test
- 🚧 Release v0.0.2 – GitHub Release + Unity package

</details>

<details>
<summary><strong>v0.1.0 – Pre-Alpha</strong></summary>

- 📋 First public release – single-prop inside-out + outside-in rigid bodies
- 📋 Documentation updates
- 📋 Example scenes

</details>

<details>
<summary><strong>v0.2.0–v0.9.x – Pre-Alpha</strong></summary>

- 📋 Markerless bodies (outside-in)
- 📋 UWB fusion + auto-calibration
- 📋 Multi-person support
- 📋 Performance optimizations

</details>

<details>
<summary><strong>v1.0.0 – Alpha</strong></summary>

- 📋 Full multi-person outside-in, stable inside-out point tracking, production-ready package
- 📋 Full documentation
- 📋 Comprehensive test coverage

</details>

<details>
<summary><strong>v1.x.x – Alpha</strong></summary>

- 📋 Hand/face tracking
- 📋 Live retargeting UI
- 📋 Performance improvements
- 📋 Community feedback integration

</details>

<details>
<summary><strong>v2.0.0+ – Future</strong></summary>

- 📋 SMPL-X meshes
- 📋 Real-time denoising
- 📋 Mobile companion app integration
- 📋 API stability
- 📋 Long-term support

</details>

---

## 💬 Support

* **Issues:** github.com/ajcampbell1333/Proptical_Unity/issues
* **Discussions:** github.com/ajcampbell1333/Proptical_Unity/discussions
* **Unreal Version:** github.com/ajcampbell1333/Proptical

---

## 📄 License

Copyright (c) 2025 AJ Campbell

Licensed under the MIT License. See LICENSE for details.

---

## 🤝 Contributing

Proptical is open-source under the MIT License. Got ideas for how to make Proptical better? Contributions are welcome!

<details>
<summary><strong>Development Workflow</strong></summary>

1. Fork this repository and clone it (`git clone https://github.com/your-username/Proptical_Unity.git`)
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Add your changes (`git add .`)
4. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
5. Push to the branch (`git push origin feature/AmazingFeature`)
6. Open a Pull Request

</details>

<details>
<summary><strong>What We Welcome</strong></summary>

* New model containers
* Better ESP32 firmware
* Unity-specific optimizations
* Calibration UI improvements
* Demo scenes (sword fights, virtual production, etc.)

</details>

---

## 👤 Credits

Created by **AJ Campbell**.

---

_Proptical: Free Open-Source MOCAP optical+RF Inside-out-OR-Outside-in full-body-OR-prop tracking
