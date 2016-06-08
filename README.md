# Unity Speedometer

A simple project showing how to use location services to measure speed. The sample app reports helpful data (location coordinates, accuracy, sampling timeframe)

## Accuracy

Deriving speed from location data works better at higher speed (say, over 20 km/h).

- Location services do not provide samples at regular intervals. Instead, samples are delivered as a result of the user moving around (getting a new sample while not moving can take up to a minute, in the meantime speed won't update).
- Location accuracy is limited. In a best case scenario (outdoors, clear sky), the maximum reported accuracy is ~5m.
- New samples reflecting increased accuracy may be provided out of turn. In a typical scenario the user move from indoors to outdoors, causing a not-same-accuracy sample to arrive, which then produces meaningless speed variations.

Backlogged  samples may be considered to refine the output. This approach is implemented using a confidence parameter.
Not receiving location updates while not moving is a problem. This is crudely addressed using a timeout ([an alternative][4]). 

With newer devices it is possible to leverage [Core Motion][1] or [equivalent][2] via a [custom plugin][3].

The method used here does not aim at producing smooth results. If you need smooth results, consider using extrapolation or accelerometric data.

## Acknowledgements

To extract distances from geographic coordinates, I adapted [nifty JS code by Chris Veness] (http://www.movable-type.co.uk/scripts/latlong.html) at movable-type.co.uk

[1]: https://developer.apple.com/library/ios/documentation/CoreMotion/Reference/CoreMotion_Reference/
[2]: http://stackoverflow.com/questions/4993993/how-to-detect-walking-with-android-accelerometer
[3]: http://codereview.stackexchange.com/questions/110317/unity3d-native-ios-plug-in-to-read-pedometer-data
[4]: http://stackoverflow.com/questions/21957588/detecting-when-someone-begins-walking-using-core-motion-and-cmaccelerometer-data
