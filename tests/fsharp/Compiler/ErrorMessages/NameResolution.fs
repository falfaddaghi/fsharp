// Copyright (c) Microsoft Corporation.  All Rights Reserved.  See License.txt in the project root for license information.

namespace FSharp.Compiler.UnitTests

open NUnit.Framework
open FSharp.Compiler.SourceCodeServices

[<TestFixture>]
module ``Name Resolution`` =

    [<Test>]
    let ``Field not in record``() =    
        CompilerAssert.TypeCheckSingleError
            """
type A = { Hello:string; World:string }
type B = { Size:int; Height:int }
type C = { Wheels:int }
type D = { Size:int; Height:int; Walls:int }
type E = { Unknown:string }
type F = { Wallis:int; Size:int; Height:int; }

let r:F = { Size=3; Height=4; Wall=1 }
            """
            FSharpErrorSeverity.Error
            1129
            (9, 31, 9, 35)
            "The record type 'F' does not contain a label 'Wall'."

    [<Test>]
    let ``Record field proposal``() =    
        CompilerAssert.TypeCheckSingleError
            """
type A = { Hello:string; World:string }
type B = { Size:int; Height:int }
type C = { Wheels:int }
type D = { Size:int; Height:int; Walls:int }
type E = { Unknown:string }
type F = { Wallis:int; Size:int; Height:int; }

let r = { Size=3; Height=4; Wall=1 }
            """
            FSharpErrorSeverity.Error
            39
            (9, 29, 9, 33)
            "The record label 'Wall' is not defined."

    [<Test>]
    let ``Global qualifier after dot``() =    
        CompilerAssert.TypeCheckWithErrors
            """
let x = global.System.String.Empty.global.System.String.Empty
            """
            [| FSharpErrorSeverity.Error, 1126, (2, 36, 2, 42), " may only be used as the first name in a qualified path"
               FSharpErrorSeverity.Error, 39, (2, 43, 2, 49), "The field, constructor or member 'System' is not defined." |]

            

