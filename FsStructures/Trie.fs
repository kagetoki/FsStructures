namespace FsStructures
    open System.Diagnostics

    [<DebuggerDisplay("{key} | Count: {children.Count}")>]
    type trie = 
        {
            key : char option
            isWordEnd : bool
            children : Map<char,trie>
        }
    
    [<RequireQualifiedAccess>]
    module Trie = 
        let private child key tree =
            Map.tryFind key tree.children

        let rec private getRec prefix prefixAcc result tree = 
            let prefixAcc = match tree.key with
                            | None -> prefixAcc
                            | Some c -> prefixAcc + c.ToString()
            let result = if tree.isWordEnd then prefixAcc :: result
                         else result
                
            match prefix with
            | "" | null -> let childrenResult = 
                                [for c in tree.children do 
                                    yield getRec prefix prefixAcc result c.Value
                                ]
                           let r = List.concat childrenResult |> List.append result
                           r
            | str -> let key = str.[0]
                     let child = child key tree
                     match child with
                     | None -> result
                     | Some c -> getRec str.[1..] prefixAcc result c
        
        let get prefix tree = 
            getRec prefix "" [] tree

        let private createNode key isEnd = 
            {key = Some key; isWordEnd = isEnd; children = Map.empty}

        let rec private addRec str tree = 
            match str with
            | null -> failwith "can't insert null"
            | "" ->  ({tree with isWordEnd = true})
            | str -> let child = 
                        match tree.children.TryFind str.[0] with
                        | Some t -> t
                        | None -> createNode str.[0] false
                     let child1 = addRec str.[1..] child
                     let children1 = Map.add str.[0] child1 tree.children
                     let result = {tree with children = children1}
                     result
        
        let add str tree = 
            match tree.key with
            | Some c -> failwith "only root can be used to insert"
            | None -> match str with
                      | "" | null -> tree
                      | str -> let result = addRec str tree 
                               result
        
    type trie with member this.get str = Trie.get str this
                   static member root = {key=None; children = Map.empty; isWordEnd = false }