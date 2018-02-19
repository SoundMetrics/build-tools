// Copyright (c) 2012-2018 Sound Metrics. All Rights Reserved. 

namespace BuildInfo

module Streams =

    open System.IO

    let areIdentical (s1 : Stream) (s2 : Stream) =

        if s1.Length <> s2.Length then
            false
        else
            let mutable filesAreIdentical = true
            let mutable bytesLeft = s1.Length

            while bytesLeft > 0L && filesAreIdentical do
                if s1.ReadByte() <> s2.ReadByte() then
                    filesAreIdentical <- false

                bytesLeft <- bytesLeft - 1L

            filesAreIdentical
