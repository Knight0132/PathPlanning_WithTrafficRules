# Path Planning With Traffic Rules

![Unity](https://img.shields.io/badge/Unity-2021.3.32f1c1-black?style=flat&logo=unity)
![License](https://img.shields.io/badge/License-MIT-blue.svg)

A simulation project for robot path planning in warehouse environments, considering complex indoor traffic rules. This repository supports a Bachelor's degree thesis focusing on the integration of traffic constraints into indoor map data structures.

## 📖 About The Project

This project simulates robot navigation in an indoor environment (like a warehouse) where traffic rules significantly impact efficiency and safety. Unlike traditional path planning that only considers distance, this system accounts for:

- **Speed Limits**: Different areas have specific speed restrictions.
- **Intersections**: Robots must decelerate when approaching or crossing intersections.
- **Traffic Direction**: One-way aisles and specific traffic flows.

The simulation compares traditional algorithms with traffic-aware modifications to demonstrate the effectiveness of the proposed Indoor Map Data Structure.

## 🚀 Features & Algorithms

### 1. Traditional A\* Algorithm

- **Cost Function $g(x)$**: Based purely on Euclidean distance.
- **Heuristic $h(x)$**: Euclidean distance to the goal.
- **Behavior**: Finds the shortest path geographically but ignores speed limits and traffic rules.

### 2. Improved A\* (Traffic-Aware)

- **Cost Function**: Time-based cost ($time = distance / speed$).
- **Dynamic Speed**:
  - Takes input speed parameters.
  - Multiplies speed by a **deceleration factor** when neighbors are intersections.
  - Adheres to speed limits defined in the map data.
- **Goal**: Find the _fastest_ path rather than just the shortest one, ensuring compliance with safety rules.

## 🛠 Settings & Requirements

- **Platform**: Unity3D
- **Unity Version**: **2021.3.32f1c1** or compatible.
- **Language**: C#

## 📦 Indoor Map Data Structure

This project utilizes a specialized JSON schema for indoor mapping that embeds traffic rules.

- **Schema Reference**: [indoorjson3-python](https://github.com/Knight0132/indoorjson3-python)

The data structure defines:

- **CellSpaces**: Walkable areas.
- **Traffic Rules**: Speed limits, one-way constraints.
- **Connections**: Topology of the indoor space.

## 🧪 Experiments & Results

We tested the model on a **25x20** grid map representing a warehouse.

- **Default Robot Speed**: 7 units/s
- **Scenario**: Specific areas have strict speed limits and intersections require deceleration.

### Results

The traffic-aware algorithm generates different paths compared to the traditional one. By avoiding high-traffic or low-speed areas, it optimizes for **time and legality** rather than just distance, making it more suitable for multi-robot management.

### Demos

| Path Planning Demo 1                                                                                              | Path Planning Demo 2                                                                                              |
| ----------------------------------------------------------------------------------------------------------------- | ----------------------------------------------------------------------------------------------------------------- |
| https://github.com/Knight0132/PathPlanning_WithTrafficRules/assets/104136770/c6893759-c640-46a3-b6ff-a6a41cc14076 | https://github.com/Knight0132/PathPlanning_WithTrafficRules/assets/104136770/641a2901-e609-45d1-a50b-64dea2236f1e |

_(Click links to view if videos don't autoplay)_

## 🔧 Installation & Usage

1. **Clone the repository**
   ```bash
   git clone https://github.com/Knight0132/PathPlanning_WithTrafficRules.git
   ```
2. **Open in Unity**
   - Launch Unity Hub.
   - Click **Open** and select the cloned folder `PathplanningWithTrafficRules`.
   - Wait for Unity to import assets and compile scripts.
3. **Run the Simulation**
   - Open the primary Scene from `Assets/Scenes`.
   - Press the **Play** button in the Unity Editor.

## 📄 License

Distributed under the MIT License. See `LICENSE` for more information.

## 👥 Contact

**Knight_bluu**  
Project Link: [https://github.com/Knight0132/PathPlanning_WithTrafficRules](https://github.com/Knight0132/PathPlanning_WithTrafficRules)
