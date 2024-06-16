import Header from "../components/Header";
import dropdown from "../assets/dropdown.svg"
import rock from "../assets/rock.svg"
import paper from "../assets/paper.svg"
import scissors from "../assets/scissors.svg"
import { useState, useEffect } from "react";
import { StatsType } from "../Types";
import { useNavigate } from "react-router-dom";
import ValidateUser from "../components/ValidateUser";
import { Pie } from "react-chartjs-2";
import { Chart, ArcElement, Colors, Legend, Color } from "chart.js";
import ChartDataLabels from 'chartjs-plugin-datalabels'

/*
TODO:
- navigation
- using real values
*/

export default function Stats() {

  const [userStats, setUserStats] = useState<StatsType[]>();
  const [level, setLevel] = useState(0);
  const levelNames = ["Beginner", "Intermediate", "Advanced"]
  const [showLevels, setShowLevels] = useState(false)
  const navigate = useNavigate();
  useEffect(() => {
    ValidateUser(navigate);

    fetchStats();
    async function fetchStats() {
      const request = await fetch("http://localhost:5000/stats", {
        method: "POST",
        headers: { 
          "Content-Type" : "application/json",
          "Authorization" : `bearer ${localStorage.getItem("token")}` // Must exist because of ValidateUser
        },
        body: JSON.stringify({"Username" : localStorage.getItem("username")})
      })
      if (!request.ok) {
        console.log('woops')
        throw new Error(request.statusText);
      }
      else {
        const response = await request.json() as StatsType[]
        console.log(localStorage.getItem("username"));
        setUserStats(response);
      }
    }
  }, [])


  useEffect(() => {
    console.log(userStats);
  }, [userStats])

  const [showPerformance, setShowPerformance] = useState(true)

  return (
    <div className="flex flex-col items-center px-5 pt-2 pb-5 w-screen">
      <span className='flex flex-row justify-end w-full pb-16'>
        <button className="text-zinc-500 font-bold mr-4">Play</button>
        <button className="text-zinc-500 font-bold">Sign out</button>
      </span>
      <Header />
      <div className="flex flex-row items-center w-3/4 justify-evenly font-bold mt-5">
        <h1>Player</h1>
        <p>vs</p>
        <div>
          <button onClick={() => setShowLevels(prev => !prev)} className="flex flex-row items-center">
            <h1 className="mr-3">{levelNames[level]}</h1>
            <img className={`w-3 ${!showLevels && "rotate-180"}`} src={dropdown} />
          </button>
          {showLevels && 
            <div className="absolute pt-3">
              <ul className="bg-[#202020] rounded-xl p-2 flex flex-col w-32 items-start">
                {levelNames.map((levelName, index) => {
                  if (index != level) return (<button key={levelName} onClick={() => {setShowLevels(false); setLevel(index)}}><h1>{levelName}</h1></button>)
                })}
              </ul>
            </div>
          }
        </div>
      </div>
      <span className='flex flex-row w-3/4 justify-between mt-5'>
        <button className="font-bold" onClick={() => setShowPerformance(true)}>Performance</button>
        <button className="font-bold" onClick={() => setShowPerformance(false)}>Analysis</button>
      </span>
      {userStats && (showPerformance ? <Performance userStats={userStats[level]}/> : <Analysis userStats={userStats[level]}/>)}
    </div>
  )
}

type PerformanceProps = {
  userStats: StatsType
}

function Performance({ userStats } : PerformanceProps) {

  useEffect(() => {
    console.log(userStats);
  }, [userStats])

  return (
    <div className="mt-5 w-full flex flex-col items-center">
      <span className="flex flex-col items-center font-bold mb-5">
        <h1 className="text mb-3">Win Rate</h1>
        <p className="text-3xl">{Math.round(userStats.winRate*100)} %</p>
      </span>
      <span className="flex flex-col items-center font-bold mb-5">
        <h1 className="text mb-3">Longest Win Streak</h1>
        <p className="text-3xl">{userStats.longestStreak} Wins</p>
      </span>
      {/*<Leaderboard />*/}
    </div>
  )

  function Leaderboard() {

    type leaderboardStats = {
      username: string,
      winrate: number
    }

    // map over Anya 96 % bit
    const [leaderboardEntries, setLeaderboardEntries] = useState<leaderboardStats[]>([{"username" : "Anya", "winrate" : 96}, {"username" : "Rierra", "winrate" : 96}])

    return (
      <div className='w-full flex flex-col items-center'>
        <h1 className="font-bold mb-5">Leaderboard</h1>
        <table className='text-left w-3/4 border-collapse border border-zinc-400 bg-[#404040] rounded-sm'>
          <thead>
            <tr className="font-bold italic text-zinc-200 text-sm">
              <th className="border border-zinc-400 p-1">Username</th>
              <th className="border border-zinc-400 p-1">Win Rate</th>
            </tr>
          </thead>
          <tbody>
            {leaderboardEntries.map(entry => {
              return (
                <tr className="font-bold">
                  <td className="border border-zinc-400 p-1">{entry.username}</td>
                  <td className="border border-zinc-400 p-1">{entry.winrate} %</td>
                </tr>
              )
            })}
          </tbody>
        </table>
      </div>
    )
  }
}

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
    if (userStats.ace == "rock") { setAce(rock) }
    else if (userStats.ace == "paper") { setAce(paper) }
    else if (userStats.ace == "scissors") { setAce(scissors) }
    else { console.error("Invalid ace.") }

    if (userStats.nemesis == "rock") { setNemesis(rock) }
    else if (userStats.nemesis == "paper") { setNemesis(paper) }
    else if (userStats.nemesis == "scissors") { setNemesis(scissors) }
    else { console.error("Invalid nemesis.") }
  }, [userStats])

  return (
    <div className="w-full mt-5">
      <div className="flex flex-col items-center justify-evenly text-lg font-bold">
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-5">Choice Distribution</h1>
          <span className="w-48">{choiceDistData && <Pie data={choiceDistData} options={options}/>}</span>
        </span>
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-5">Ace</h1>
          {userStats.ace != "none" ? <img className="w-24" src={ace} /> : <h1>None</h1>}
        </span>
        <span className="flex flex-col items-center mb-10">
          <h1 className="mb-5">Nemesis</h1>
          {userStats.nemesis != "none" ? <img className="w-24" src={nemesis} /> : <h1>None</h1>}
        </span>
        <span className="flex flex-col items-center mb-5">
          <h1 className="mb-10">Playstyle</h1>
          <p className="text-3xl capitalize">{userStats.style}</p>
        </span>
      </div>
    </div>
  )
}