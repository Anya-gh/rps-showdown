import { StatsType } from "../Types";
import { useState, useEffect } from "react";
import { Chart, ArcElement, Colors, Legend, Color } from "chart.js";
import { Pie } from "react-chartjs-2";
import ChartDataLabels from 'chartjs-plugin-datalabels'
import rockIcon from "../assets/rock.svg"
import paperIcon from "../assets/paper.svg"
import scissorsIcon from "../assets/scissors.svg"
import Card from "./Card";

type AnalysisProps = {
  userStats: StatsType
}

function Analysis({ userStats } : AnalysisProps) {

  Chart.register(ArcElement);
  Chart.register(Colors);
  Chart.register(Legend);
  Chart.register(ChartDataLabels);

  const [ace, setAce] = useState<string>()
  const [nemesis, setNemesis] = useState<string>()

  type PieChartData = {
    datasets: {
      data: number[],
    }[],
    labels: string[]
  }
  const choiceDistData: PieChartData = {
    datasets : [{
      data: [userStats.choiceDistribution.rock, userStats.choiceDistribution.paper, userStats.choiceDistribution.scissors],
    }],
    labels: ["Rock", "Paper", "Scissors"]
  }

  type PieChartOptions = {
    responsive: boolean,
    plugins : {
      legend: {
        position: "top" | "bottom" | "right" | "left",
        labels : {
          color: Color
        }
      },
      datalabels: {
        color: Color,
        formatter: (value:number) => string
      }
    }
  }

  const options: PieChartOptions = {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
        labels : {
          color: "#c5c5c5" 
        }
      },
      datalabels : {
        color: "#e9e9e9",
        formatter: function (value:number) {
          return `${Math.round(value * 100)}%`;
        },
      }
    }
  }

  useEffect(() => {
    if (userStats.ace == "rock") { setAce(rockIcon) }
    else if (userStats.ace == "paper") { setAce(paperIcon) }
    else if (userStats.ace == "scissors") { setAce(scissorsIcon) }
    else { console.error("Invalid ace.") }

    if (userStats.nemesis == "rock") { setNemesis(rockIcon) }
    else if (userStats.nemesis == "paper") { setNemesis(paperIcon) }
    else if (userStats.nemesis == "scissors") { setNemesis(scissorsIcon) }
    else { console.error("Invalid nemesis.") }
  }, [userStats])

  return (
    <div className="w-full mt-5">
      <div className="flex flex-col md:flex-row items-center justify-evenly text-lg font-bold">
        <Card>
          <span className="flex flex-col items-center mb-5">
            <h1 className="mb-5 text-lg">Choice Distribution</h1>
            <span className="w-40">{choiceDistData && <Pie data={choiceDistData} options={options}/>}</span>
          </span>
        </Card>
        <Card>
          <span className={`flex flex-col items-center mb-5 ${userStats.ace == "none" && "justify-center h-full"}`}>
            <h1 className={`text-lg ${userStats.ace != "none" ? "mb-5" : "mb-auto"}`}>Ace</h1>
            {userStats.ace != "none" ? <img className="w-24" src={ace} /> : <h1 className="text-3xl mb-auto">None</h1>}
          </span>
        </Card>
        <Card>
          <span className={`flex flex-col items-center mb-5 ${userStats.nemesis == "none" && "justify-center h-full"}`}>
            <h1 className={`text-lg ${userStats.nemesis != "none" ? "mb-5" : "mb-auto"}`}>Nemesis</h1>
            {userStats.nemesis != "none" ? <img className="w-24" src={nemesis} /> : <h1 className="text-3xl mb-auto">None</h1>}
          </span>
        </Card>
        <Card>
        <div className="flex flex-col items-center justify-center h-full">
            <h1 className="text-lg mb-auto">Playstyle</h1>
            <p className="text-3xl mb-auto capitalize">{userStats.style}</p>
        </div>
        </Card>
      </div>
    </div>
  )
}

export default Analysis;