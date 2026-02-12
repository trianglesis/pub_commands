state.vc4hdmi0 {
        control.1 {
                iface CARD
                name 'HDMI Jack'
                value false
                comment {
                        access read
                        type BOOLEAN
                        count 1
                }
        }
        control.2 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                value.2 0
                value.3 0
                value.4 0
                value.5 0
                value.6 0
                value.7 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 8
                        range '0 - 36'
                }
        }
        control.3 {
                iface PCM
                name 'IEC958 Playback Mask'
                value ffffffffffffffffffffffffffffffffffffffffffffffff0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
                comment {
                        access read
                        type IEC958
                        count 1
                }
        }
        control.4 {
                iface PCM
                name 'IEC958 Playback Default'
                value '0400000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000'
                comment {
                        access 'read write'
                        type IEC958
                        count 1
                }
        }
        control.5 {
                iface PCM
                name ELD
                value '0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000'
                comment {
                        access 'read volatile'
                        type BYTES
                        count 128
                }
        }
}
state.vc4hdmi1 {
        control.1 {
                iface CARD
                name 'HDMI Jack'
                value false
                comment {
                        access read
                        type BOOLEAN
                        count 1
                }
        }
        control.2 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                value.2 0
                value.3 0
                value.4 0
                value.5 0
                value.6 0
                value.7 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 8
                        range '0 - 36'
                }
        }
        control.3 {
                iface PCM
                name 'IEC958 Playback Mask'
                value ffffffffffffffffffffffffffffffffffffffffffffffff0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
                comment {
                        access read
                        type IEC958
                        count 1
                }
        }
        control.4 {
                iface PCM
                name 'IEC958 Playback Default'
                value '0400000100000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000'
                comment {
                        access 'read write'
                        type IEC958
                        count 1
                }
        }
        control.5 {
                iface PCM
                name ELD
                value '0000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000'
                comment {
                        access 'read volatile'
                        type BYTES
                        count 128
                }
        }
}
state.Device {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value false
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 16
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -700
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 14
                value.1 14
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -2300
                        dbvalue.1 -2300
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 35
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 2300
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
state.Device_1 {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value false
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 16
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -700
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 14
                value.1 14
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -2300
                        dbvalue.1 -2300
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 35
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 2300
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
state.Device_2 {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value false
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 16
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -700
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 14
                value.1 14
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -2300
                        dbvalue.1 -2300
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 35
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 2300
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
state.USBCard2 {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -9999999
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 32
                value.1 32
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -500
                        dbvalue.1 -500
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 -1200
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
state.USBCard1 {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value false
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -9999999
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 32
                value.1 32
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -500
                        dbvalue.1 -500
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 -1200
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
state.USBCard3 {
        control.1 {
                iface PCM
                name 'Playback Channel Map'
                value.0 0
                value.1 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 2
                        range '0 - 36'
                }
        }
        control.2 {
                iface PCM
                name 'Capture Channel Map'
                value 0
                comment {
                        access 'read volatile'
                        type INTEGER
                        count 1
                        range '0 - 36'
                }
        }
        control.3 {
                iface MIXER
                name 'Mic Playback Switch'
                value false
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.4 {
                iface MIXER
                name 'Mic Playback Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 31'
                        dbmin -9999999
                        dbmax 800
                        dbvalue.0 -9999999
                }
        }
        control.5 {
                iface MIXER
                name 'Speaker Playback Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.6 {
                iface MIXER
                name 'Speaker Playback Volume'
                value.0 32
                value.1 32
                comment {
                        access 'read write'
                        type INTEGER
                        count 2
                        range '0 - 37'
                        dbmin -9999999
                        dbmax 0
                        dbvalue.0 -500
                        dbvalue.1 -500
                }
        }
        control.7 {
                iface MIXER
                name 'Mic Capture Switch'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
        control.8 {
                iface MIXER
                name 'Mic Capture Volume'
                value 0
                comment {
                        access 'read write'
                        type INTEGER
                        count 1
                        range '0 - 35'
                        dbmin -1200
                        dbmax 2300
                        dbvalue.0 -1200
                }
        }
        control.9 {
                iface MIXER
                name 'Auto Gain Control'
                value true
                comment {
                        access 'read write'
                        type BOOLEAN
                        count 1
                }
        }
}
