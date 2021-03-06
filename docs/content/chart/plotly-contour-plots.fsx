﻿(*** hide ***)
#I "../../../bin"
#I "../../../packages/MathNet.Numerics/lib/net40"
#load "../credentials.fsx"
#r "XPlot.Plotly.dll"
#r "XPlot.Plotly.WPF.dll"
#r "MathNet.Numerics.dll"

open XPlot.Plotly
open MathNet.Numerics
open MathNet.Numerics.Distributions
open System

Plotly.Signin MyCredentials.userAndKey

let size = 100

let x = Generate.LinearSpaced(size, -2. * Math.PI, 2. * Math.PI)
let y = Generate.LinearSpaced(size, -2. * Math.PI, 2. * Math.PI)
let z = Array2D.create size size 0.

for i in 0 .. 99 do
    for j in 0 .. 99 do
        let r2 = x.[i] ** 2. + y.[j] ** 2.
        z.[i,j] <- sin x.[i] * cos y.[j] * sin r2 / log(r2 + 1.)

let t = Generate.LinearSpaced(2000, -1., 1.2)

let normal = new Normal(0., 1.0)

let sample =
    normal.Samples()
    |> Seq.take 2000
    |> Seq.toArray

let x1 = Array.mapi (fun i x -> t.[i] ** 3. + 0.3 * x) sample
let y1 = Array.mapi (fun i x -> t.[i] ** 6. + 0.3 * x) sample

(**
Plotly Contour Plots
====================

Basic Contour Plot
------------------
*)

let trace =
    Contour(
        z = z,
        x = x,
        y = y
    )

Figure(Data.From [trace])

(**
<iframe width="640" height="480" frameborder="0" seamless="seamless" scrolling="no" src="https://plot.ly/~TahaHachana/268.embed?width=640&height=480" ></iframe>
*)

(**
2D Histogram Contour Plot with Histogram Subplots
-------------------------------------------------
*)

let trace1 =
    Scatter(
        x = x1,
        y = y1,
        mode = "markers",
        name = "points",
        marker =
            Marker(
                color = "rgb(102,0,0)",
                size = 2,
                opacity = 0.4
            )
    )

let trace2 =
    Histogram2dContour(
        x = x1,
        y = y1,
        name = "density",
        ncontours = 20.,
        colorscale = "Hot",
        reversescale = true,
        showscale = false
    )

let trace3 =
    Histogram(
        x = x1,
        name = "x density",
        marker = Marker(color = "rgb(102,0,0)"),
        yaxis = "y2"
    )

let trace4 =
    Histogram(
        y = y1,
        name = "y density",
        marker = Marker(color = "rgb(102,0,0)"),
        xaxis = "x2"
    )

let data' = Data [trace1; trace2; trace3; trace4]

let layout =
    Layout(
        showlegend = false,
        autosize = false,
        width = 600.,
        height = 550.,
        xaxis =
            XAxis(
                domain = [|0.; 0.85|],
                showgrid = false,
                zeroline = false
            ),
        yaxis =
            YAxis(
                domain = [|0.; 0.85|],
                showgrid = false,
                zeroline = false
            ),
        margin = Margin(t = 50.),
        hovermode = "closest",
        bargap = 0.,
        xaxis2 =
            XAxis(
                domain = [|0.85; 1.|],
                showgrid = false,
                zeroline = false
            ),
        yaxis2 =
            YAxis(
                domain = [|0.85; 1.|],
                showgrid = false,
                zeroline = false
            )
    )

Figure(data', layout)

(**
<iframe width="640" height="480" frameborder="0" seamless="seamless" scrolling="no" src="https://plot.ly/~TahaHachana/269.embed?width=640&height=480" ></iframe>
*)
