# Simple cpu raytracer

![2880x2160 64x sampling 1h render](https://raw.githubusercontent.com/ViktorHura/simple-cpu-raytracer-cs/master/renders/4K.png)

I decided to write a simple raytracer from scratch. I was heavily inspired by Eric Haines's mini-book [Ray Tracing in One Weekend](https://github.com/RayTracing/InOneWeekend) and the helpful explanation of the algorithm by [Scatchapixel](https://www.scratchapixel.com/lessons/3d-basic-rendering/introduction-to-ray-tracing/raytracing-algorithm-in-a-nutshell).

The program will divide the image into slices and distribute the rendering between the given amount of threads.

The resolution of the image and sampling rate can be set but the image is always in the 4:3 aspect ratio.

Compiled binaries are available [here](https://github.com/ViktorHura/simple-cpu-raytracer-cs/releases)

If you wish the change the scene or camera properties then you can do that in the main function of program.cs, everything there should be self explanatory. 
