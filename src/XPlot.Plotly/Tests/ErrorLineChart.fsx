#load "../packages/FSLab.0.3.20/FsLab.fsx"

open System
open XPlot.Plotly

type Chartable = {XD:DateTime; YD:float}

// Raw Data
let tD1 = [|
                        { XD = System.DateTime.Parse("5/13/2016"); YD = 2.0 };
                        { XD = System.DateTime.Parse("5/12/2016"); YD = 6.0 };
                        { XD = System.DateTime.Parse("5/9/2016");  YD = 1.0 };
                        { XD = System.DateTime.Parse("5/20/2016"); YD = 8.0 }
                            |]
// Prediction Line
let tD2 = [|
                        { XD = System.DateTime.Parse("5/9/2016");  YD = 0.0 };
                        { XD = System.DateTime.Parse("5/12/2016");  YD = 2.0 };
                        { XD = System.DateTime.Parse("5/13/2016");  YD = 6.0 };
                        { XD = System.DateTime.Parse("5/20/2016"); YD = 7.0 };
                        { XD = System.DateTime.Parse("5/22/2016"); YD = 7.0 }
                            |]
// Upper Bound
let tD3 = [|
                        { XD = System.DateTime.Parse("5/9/2016");  YD = 1.6 };
                        { XD = System.DateTime.Parse("5/12/2016");  YD = 3.0 };
                        { XD = System.DateTime.Parse("5/13/2016"); YD = 7.0 };
                        { XD = System.DateTime.Parse("5/20/2016"); YD = 7.9 };
                        { XD = System.DateTime.Parse("5/22/2016"); YD = 7.9 }
                            |]
// Lower Bound
let tD4 = [|
                        { XD = System.DateTime.Parse("5/9/2016");  YD = -1.5 };
                        { XD = System.DateTime.Parse("5/12/2016");  YD = 0.9 };
                        { XD = System.DateTime.Parse("5/13/2016");  YD = 5.0 };
                        { XD = System.DateTime.Parse("5/20/2016");  YD = 6.1 };
                        // { XD = System.DateTime.Parse("5/20/2016"); YD = 7.0 };
                        { XD = System.DateTime.Parse("5/22/2016"); YD = 6.2 }
                        
                            |]

let getXVal (data:Chartable array) =
    data
    |> Array.map(fun x -> x.XD.Date) 
    |> Array.sortBy(fun x -> x)
    |> Array.toList

let getYVal (data:Chartable array) =
    data
    |> Array.sortBy(fun y -> y.XD.Date)
    |> Array.map(fun y -> y.YD) 
    |> Array.toList

let rawDataX = 
    tD1
    |> getXVal

let rawDataY = 
    tD1
    |> getYVal

let predLineX = 
    tD2
    |> getXVal

let predLineY = 
    tD2
    |> getYVal

let upErrX = 
    tD3
    |> getXVal

let upErrY = 
    tD3
    |> getYVal

let loErrX = 
    tD4
    |> getXVal

let loErrY = 
    tD4
    |> getYVal

let rawData =
    Scatter(
        x = rawDataX,
        y = rawDataY,
        (*
        error_y {
            type = 'data',
            value = 10,
            visible = true;
        },
        *)
        
        mode = "lines+markers",
        text = ["y=mx+b"],
        name = "Raw Data"
        
    )
let predictionLine =
    Scatter(
        x = predLineX,
        y = predLineY, 
        mode = "lines+markers",
        name = "Prediction Line"
    )
let upperError =
    Scatter(
        x = upErrX,
        y = upErrY,
        fill = "tonexty", 
        fillcolor = "rgba(255, 255, 153, 0.2)", 
        mode = "text",
        name = "Upper Bound"
    )
let lowerError =
    Scatter(
        x = loErrX,
        y = loErrY,
        fill = "tonexty", 
        fillcolor = "rgba(255, 255, 153, 0.2)",
        mode = "text", 
        name = "Lower Bound"
    )

[rawData; predictionLine; upperError; lowerError;]
|> Plotly.Plot

