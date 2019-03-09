## Test cases

- Tab navigation
- Control template honors appearance properties (alignment, background, foreground, borders, etc)
- Control template honors text properties (font family, font size, etc)
- Mouse interaction
- Keyboard interaction
- Touch interaction
- Enabled/disabled
- Variable item size
- Control responds correctly to size changes
- Control responds correctly to items list changes
- Control responds correctly to selected item/index changes

## Features

- Fully templatable
- Inline vs. flyout
- Fully globalized
- RTL support
- Accessibility

## Properties

MinValue (TimeSpan; default TimeSpan.Zero)
MaxValue (TimeSpan; default TimeSpan.Infinite)
    < 1 day hides day selector
    < 1 hour hides hour selector
    < 1 minute hides minute selector
Value (TimeSpan?)

MinuteIncrement (1, 5, 10, 15, 20, 30; default 1)
SecondIncrement (1, 5, 10, 15, 20, 30; default 1)

Precision (Days, Hours, Minutes, Seconds; each level hides smaller selectors; default Minutes)

MaxValue and Precision must not conflict!

## Implementation

Four selectors: Days, Hours, Minutes, Seconds
No milliseconds; if you want that kind of precision, use a number input.


---------------------------
| 0 d | 3 h | 44 m | 23 s |
---------------------------

How do we get localized abbreviations for the units?

