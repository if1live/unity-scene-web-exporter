# UnitySceneWebExporter

[![Build Status](https://travis-ci.org/if1live/unity-scene-web-exporter.svg?branch=master)](https://travis-ci.org/if1live/unity-scene-web-exporter)

Export Three.js or A-Frame From Unity Scene

![screenshot](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-mini-threejs-viewer.jpg)

## Screenshot
### Unity3D scene
![Unity3D scene](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/sample-scene-unity.jpg)

### Three.js
![Three.js](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/sample-scene-threejs.jpg)

### A-Frame
![A-Frame](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/sample-scene-aframe.jpg)

## Feature
* Export Scene to Three.js scene format (THREE.ObjectLoader can load exported data)
* Export Scene to A-Frame document
* Export C# Script variable
* Support lightmapping

## 
2. Edit->Kanau->"Export AFrame" or "Export Three.js"
3. Export


## How to use
### Install
Open your Unity3D project. Copy `/UnityProject/Assets/Kanau" directory into your project's assets directory.

![copy kanau](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-copy-kanau.png)

### Lightmapping (Optional)
If you don't want to export lightmapping, skip it.
`/UnityProject/Assets/Scenes/DemoLightmap` is Lightmap Sample Scene.

Set Light as `Baked`.

![baked light](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-light-baked.png)

Set GameObject as `Static`.

![set static](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-set-static.png)

Disable Automatic lightmap build to make exr files. Then, build lightmap.

* Does <scene_name>/Lightmap-<num>_comp_dir.exr exist?
* Does <scene_name>/Lightmap-<num>_comp_light.exr exist?
* If automatic lightmap enabled, exr files doesn't exist.

![build lightmap](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-build-lightmap.png)

### Export
`Edit` -> `Kanau` -> `Export Aframe` or `Export Three.js`. Click `Export` and select target filepath.

![kanau menu](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-kanau-menu.jpg)

* Note : Unselect objects from Hierarchy view. If some objects are selected, only those are exported. (unselect means export all objects)

### View (A-Frame)
Open exported html in browser. 

### View (Three.js)
1. Export scene as `scene.json`. (hardcoded in viewer html)
2. Copy exported file into `/MiniThreejsViewer`. (json file, images directory, models directory)
3. Open `/MiniThreejsViewer/index.html`.
 
![screenshot](https://raw.githubusercontent.com/if1live/unity-scene-web-exporter/master/document/manual-mini-threejs-viewer.jpg)

## Note
* Tested on Unity3D 5.4.2p3.
* Export configurations are used in A-Frame exporting. Three.js doesn't use it.
* A-Frame is unstable library. (current A-Frame version is 0.3.2) In future, A-Frame exporter wiil be broken.

## Similar Projects / Libraries
* [J3D - unity3d-to-threejs exporter](https://github.com/drojdjou/J3D)
* [UnityAFrameExporter - Export A-Frame From Unity Scene](https://github.com/umiyuki/UnityAFrameExporter)
* [Three.js JSON Exporter - via Unity Asset Store](https://www.assetstore.unity3d.com/en/#!/content/40550)
* [unity-webvr - Export Unity scene to WebGL](https://github.com/xirvr/unity-webvr)
