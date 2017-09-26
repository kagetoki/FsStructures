// Learn more about F# at http://fsharp.org

open System
open FsStructures

[<EntryPoint>]
let main argv =
    let root = Trie.add "123" trie.root
    let root = Trie.add "234" root
    let root = Trie.add "122" root
    let root = Trie.add "1224" root
    let root = Trie.add "1236" root
    let select = root.get "12" 
    
    for s in select do
        Console.WriteLine s
    Console.WriteLine "done"
    Console.ReadLine() |> ignore
    0 // return an integer exit code
