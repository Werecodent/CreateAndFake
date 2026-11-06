# Limiter

The `Limiter` is a very basic but necessary tool for handling forms of repetition with a specified limit:

* `Repeat` - Keeps repeating an action until the limit is reached.
* `Retry` - Repeats an action if it encounters an exception until it passes or the limit is reached.
* `StallUntil` - Repeats an action until it returns true or the limit is reached.

Since the class is async, the above actions can be blocking or not. The limit is specified upon creating a `Limiter` instance, but some defaults are provided on the Limiter itself:

* `Limiter.Once` - 1 try.
* `Limiter.Few` - 5 tries.
* `Limiter.Dozen` - 12 tries.
* `Limiter.Myriad` - 1000 tries.
* `Limiter.Quick` - 0.5 seconds with 20 ms delay between attempts.
* `Limiter.Slow` - 5 seconds with a 200 ms delay between attempts.
