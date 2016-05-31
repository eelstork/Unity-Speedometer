# Unity Speedometer

A simple project showing how to use location services to (crudely) measure speed. Additionally, the sample app reports helpful data (location coordinates, current accuracy, sampling timeframe)

## Note about accuracy

The method used in this project is very glitchy, notably when travelling at low speed (less than 10 kmph) and/or in enclosed spaces.


Specifically, the latest 2 samples provided by location services (ideally backed by a GPS but sometimes lesser methods are used) are used. There are two problems with this:

- Location services do not provide samples at regular intervals. Typically samples are delivered as a result of the user moving around (getting a new sample while not moving can take up to a minute, in the meantime speed won't update).
- New samples reflecting increased accuracy may be provided out of turn. In a typical scenario the user move from indoors to outdoors. This causes a not-same-accuracy sample to arrive, which can translate in a meaningless velocity change.

## Acknowledgements

To extract distances from geographic coordinates, I adapted [nifty JS code by Chris Veness] (http://www.movable-type.co.uk/scripts/latlong.html) at movable-type.co.uk
