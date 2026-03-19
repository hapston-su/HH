<h1 align="center">Almost Out</h1>
<p align="center"><strong>VR Horror Escape Game</strong></p>

<p align="center">
  A survival horror VR experience where the player must escape a haunted house within <strong>3 minutes</strong>.
</p>

---

## Overview

**Almost Out** is a **VR survival horror escape experience** where the player must survive inside a haunted house and escape before the timer runs out.

The player explores dark rooms, searches for hidden keys, interacts with objects, avoids a supernatural mannequin enemy, and uses a torch as the main survival tool.

The only way to survive is to **use the torchlight strategically, find the keys, and escape before time runs out**.

---

## Game Concept

The player starts inside a dark haunted house wearing a **VR headset**.  
The objective is to escape the house before the **3-minute countdown** ends.

To escape, the player must:

1. Pick up a **torch**
2. Turn it on
3. Keep it attached to the **left hand**
4. Search the room for a **key**
5. Open the door using the **XR right controller grab interaction**
6. Move into the second room
7. Find the second key
8. Open the final door and exit the house

If the player does not escape in time, the game ends.

---

## Hardware Requirements

| Component | Requirement |
|-----------|-------------|
| VR Headset | Meta Quest Pro |
| Controllers | Meta Quest Pro Controllers |
| Platform | Meta Oculus / VR |

---

## Software Requirements

| Tool | Version |
|------|---------|
| Game Engine | Unity 6000.3.8f1 |
| XR Framework | Unity XR Interaction Toolkit |
| VR Device Support | Meta Quest Pro |

---

## Gameplay Flow

## Scene 1 - First Room

### 1. Pick Up the Torch

At the beginning of the game, the room is dark.

The player must first pick up the **torch** to make it possible to see inside the house.

- The torch is grabbed using the VR controller
- The player switches it on
- The torch stays attached to the **left hand**

This torch is essential for both **navigation** and **survival**.

---

### 2. Search for the First Key

After picking up the torch, the player must explore the first room and search for the key needed to open the door to the next room.

The room contains several interactable objects, such as:

- A coffin
- Cupboards with **3 doors**
- Small shelves
- Other small objects and storage spaces

The player must inspect these interactables and grab the key once it is found.

---

### 3. Enemy Encounter - The Mannequin

Once the torch is picked up, a scary entity becomes active.

This enemy is a **ghost-like mannequin lady** inside the haunted house.

#### Enemy Behavior

- The mannequin follows the player
- It is attracted to the player when moving in darkness
- It becomes a constant threat while the player searches for the key

#### How to Stop the Enemy

The **only way to freeze the mannequin** is to shine the **torchlight directly at her**.

This creates a tense survival mechanic where the player must:

- Keep moving carefully
- Search objects quickly
- Use the torch to stop the enemy
- Continue progressing toward the exit

---

### 4. Open the Door

Once the player finds the first key:

1. The player moves toward the door
2. Uses the **XR right controller grab functionality**
3. Opens the door
4. Enters **Room 2**

---

## Scene 2 - Second Room

The second room continues the survival experience.

The player must:

- Search for another key
- Continue avoiding the mannequin enemy
- Use the torch to freeze the enemy when needed
- Open the second door
- Escape the haunted house

---

## Time Limit

<p>
  The entire game must be completed within <strong>3 minutes</strong>.
</p>

If the player is not out of the house within 3 minutes:

<h3>Game Over</h3>

---

## Assistance System

The game includes player support features to help if they get stuck.

### Clue Request

The player can request **help** to find the key.

An assisting person can provide:

- A clue about where the key is hidden
- Guidance to help the player continue the game

### Master Key Option

If needed, a **Master Key** can be spawned for the player.

This allows the player to continue progression even if they are unable to find the hidden key.

---

## Controls

| Action | Input |
|--------|-------|
| Grab torch | XR controller grab |
| Turn on torch | XR interaction / trigger |
| Carry torch | Attached to left hand |
| Grab key | XR controller grab |
| Open door | XR right controller grab |
| Interact with objects | XR interaction system |

---

## Core Features

- Immersive **VR horror gameplay**
- Haunted house survival experience
- Two-room escape progression
- Physics-based object interaction
- Torch-based visibility and defense mechanic
- Enemy AI that reacts to light
- Key search and door unlocking system
- 3-minute time-based challenge
- Clue assistance system
- Optional master key support

---

## Objective

> **Escape the haunted house before time runs out.**

The player must:

- Find the torch
- Search for keys
- Avoid the mannequin
- Unlock doors
- Escape within 3 minutes

---

## Project Summary

**Almost Out** is a short but intense **VR horror escape game** built in **Unity 6000.3.8f1** for **Meta Quest Pro**.

The game combines:

- exploration
- tension
- object interaction
- light-based enemy control
- timed escape mechanics

to create a focused and immersive horror experience.

---

## Future Improvements

Possible future improvements include:

- Randomized key spawn locations
- More rooms
- Additional enemy types
- More advanced AI behavior
- Sound effects and jump scares
- Difficulty modes
- Better clue system
- More interactable furniture and puzzles

---

## Repository Structure

```text
Assets/
Packages/
ProjectSettings/
Tangible Interaction/
README.md
