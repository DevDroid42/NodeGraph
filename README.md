# LumaGraph
## A Visual Data Flow "Programming Language" designed for procedural real time visuals especially for LED strips. 

https://github.com/Ethan-C-Honzik/NodeGraph/assets/19912037/8b91a3c0-cfa5-4715-9eb3-b11b04b279d7

## What is Luma Graph? 
Luma Graph is a application designed for creating procedural 1D visuals that can be used to drive LED strips or to generate reactive visuals in other applications. Real time data can be streamed over the network from custom VSTs that can be loaded into DAWS. This allows for reactive and expresive visuals that can be tied to specific elements of a track. Visuals can either be streamed real time over the network to LED controllers or they can be recorded and exported to a file. These files can then be read by other applications. These sites show two experimental browser implementations of the the file: 

https://ethan-c-honzik.github.io/RandomWebProjects/Experiments/NodeSysPhonkDemo/dist/index.html

https://ethan-c-honzik.github.io/RandomWebProjects/Experiments/ShaderPhonkDemo/dist/index.html

additionally the file can be converted to a series of videos that can be imported into applications such as blender via this application:
https://github.com/Ethan-C-Honzik/LumaGraphVideoUtility

## Features
* Save and load functionality to JSON files
* Ability to group nodes into modules that can be reused in other projects
* Ability to quickly and intuitively construct easy to debug reactive visuals for music
* Integration with Digital Audio Work stations via custom VSTs running over the network
* Ability to instance groups of nodes into units that can be instanced with specific variables on triggers such as MIDI input
* Ability to do beat detection and FFT analysis of incoming audio streams
* Ability to stream live data over the network to led lights
* Ability to render out lighting data to a file that can be directly read by other applications or converted to videos
