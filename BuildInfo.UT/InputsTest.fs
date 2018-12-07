namespace BuildInfo.UT

open BuildInfo.Inputs
open Microsoft.VisualStudio.TestTools.UnitTesting

[<TestClass>]
type InputsTest () =

    [<TestMethod>]
    member __.``Too many parts in version string`` () =

        Assert.ThrowsException<TooManyPartsInVersionString>(
                    fun () -> parseRevString "1.2.3.4" |> ignore)
            |> ignore

    [<TestMethod>]
    member __.``Three-part version string`` () =

        let revString = "10.20.30"
        let expected = 10, 20, 30
        let actual = parseRevString revString
        Assert.AreEqual(expected, actual)

    [<TestMethod>]
    member __.``Three-part version string, pre-release`` () =

        let revString = "40.50.60-alpha"
        let expected = 40, 50, 60
        let actual = parseRevString revString
        Assert.AreEqual(expected, actual)
