# 🎮 Custom Gravity Player Controller (Unity)

A physics-based third-person player controller built in Unity featuring a **dynamic gravity system**, **camera-relative movement**, and **smooth character rotation** using a Rigidbody.

---

## 🚀 Features

* 🌍 **Dynamic Gravity Direction**

  * Gravity can be changed at runtime in any direction
  * Player velocity is smoothly realigned when gravity changes

* 🎥 **Camera-Relative Movement**

  * Movement input is calculated relative to the camera’s orientation
  * Works with third-person camera setups (e.g., Cinemachine)

* 🔄 **Smooth Character Rotation**

  * Character rotates based on movement direction
  * When idle, aligns with camera forward direction

* 🪂 **Jump System**

  * Jump force is applied opposite to gravity direction
  * Maintains horizontal velocity while jumping

* 🧱 **Ground Detection**

  * Sphere cast–based ground check
  * Supports uneven and angled surfaces

* 🕹️ **Air Control**

  * Reduced movement control while airborne

---

## 🧩 Scripts Overview

### `PlayerInput.cs`

Responsible for:

* Capturing movement input (horizontal & vertical)
* Jump input handling
* Previewing gravity direction using key inputs
* Confirming gravity changes
* Broadcasting gravity change events

---

### `PlayerMotor.cs`

Responsible for:

* Rigidbody-based movement and physics handling
* Camera-relative movement calculation
* Smooth rotation using Rigidbody
* Custom gravity application
* Handling gravity direction changes
* Jump mechanics
* Ground detection

---

## 🎮 Controls

| Key           | Action                    |
| ------------- | ------------------------- |
| WASD          | Movement                  |
| Space         | Jump                      |
| I / K / J / L | Preview gravity direction |
| Enter         | Apply gravity change      |

---

## ⚙️ Setup Instructions

### 1. Player Setup

* Add a **Capsule Collider**
* Add a **Rigidbody**

  * Disable `Use Gravity`
  * Enable `Interpolation`
  * Freeze rotation (X, Y, Z)

---

### 2. Attach Scripts

* `PlayerInput`
* `PlayerMotor`

---

### 3. Camera Setup

* Use a third-person camera
* Assign the **Main Camera Transform** to:

```
PlayerMotor → Camera Transform
```

---

### 4. Initialization

Initialize the motor with input reference:

```csharp
motor.Init(playerInput);
```

---

## 🧠 System Behavior

### Movement

* Input direction is mapped relative to camera orientation
* Movement is projected onto the plane defined by gravity

### Rotation

* Player rotates toward movement direction when input is present
* When idle, aligns with the camera’s forward direction

### Gravity

* Gravity is defined as a directional vector
* When gravity changes:

  * Player orientation adapts
  * Velocity is rotated accordingly to maintain motion consistency

---

## 📌 Requirements

* Unity 2021+ (recommended 2022 or later)
* Rigidbody-based physics

---

## 🔧 Possible Extensions

* Sprint / dash mechanics
* Animation system integration
* Camera alignment with gravity
* Surface-based movement effects
* Multiplayer support

---

## 📄 License

Add your preferred license here (MIT, Apache, etc.)

---

