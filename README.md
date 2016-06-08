# Unity Speedometer

A simple project showing how to use location services to crudely measure speed. The sample app reports helpful data (location coordinates, accuracy, sampling timeframe)

## Accuracy

Deriving speed from location data works better at higher speed (say, over 20 km/h).

- Location services do not provide samples at regular intervals. Instead, samples are delivered as a result of the user moving around (getting a new sample while not moving can take up to a minute, in the meantime speed won't update).
- Location accuracy is limited. In a best case scenario (outdoors, clear sky), the maximum reported accuracy is ~5m.
- New samples reflecting increased accuracy may be provided out of turn. In a typical scenario the user move from indoors to outdoors. This causes a not-same-accuracy sample to arrive, which can translate in a meaningless velocity change.

Backlogged  samples may be considered to refine the output. This approach is implemented using a confidence parameter.
Not receiving location updates while not moving is a problem. Currently this is implemented using a timemout. See [here](http://stackoverflow.com/questions/4993993/how-to-detect-walking-with-android-accelerometer) for alternatives.

The method used here does not aim at producing smooth results. If you need smooth results, consider using extrapolation or accelerometric data.

## Acknowledgements

To extract distances from geographic coordinates, I adapted [nifty JS code by Chris Veness] (http://www.movable-type.co.uk/scripts/latlong.html) at movable-type.co.uk
