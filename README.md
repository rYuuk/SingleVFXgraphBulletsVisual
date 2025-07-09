# Bullet rendering through single VFX graph in Unity (ECS)

![Showcase](https://github.com/user-attachments/assets/64958f77-9cac-4617-98aa-c44944449be5)

## Description 

- In this unity project all the bullets visuals are rendered using a single VFX graph. 
- It is built in Unity 6000.0.46f1 but can work in later or earlier versions also, as only important requirement is to set a graphics buffer every frame used in vfx graph.
- It uses ECS but thats not a requirement.
- It draws them in single draw call, so vfx graph instancing is not required.

## Project Dependencies

- Entities
- Entities Graphics
