import { useState, useEffect } from "react"
import LoginForm from "./LoginForm"
import PlayOrStats from "./PlayOrStats"
import Header from "../components/Header"
import statsIcon from "../assets/stats.png"
import spectateIcon from "../assets/spectate.png"
import difficultyIcon from "../assets/difficulty.png"

export default function Home() {

  const [loggedIn, setLoggedIn] = useState(false)

  useEffect(() => {
    if (localStorage.getItem("token")) {
      setLoggedIn(true)
    }
  }, [setLoggedIn])

  return (
    <div className="flex flex-col items-center px-5 py-20 w-screen">
      <Header />
      {loggedIn ? <PlayOrStats setLoggedIn={setLoggedIn}/> : <LoginForm setLoggedIn={setLoggedIn} />}
      <div className="h-[30rem] flex flex-col md:flex-row md:w-[60rem] md:h-auto md:mt-20 items-center justify-evenly">
        <InfoCard title={"Track your stats"} img={statsIcon} content="Track your wins, losses and more! Reflect on your matches against AI, see where your weakness lie and improve."/>
        <InfoCard title={"Challenge different levels"} img={difficultyIcon} content="Change the difficulty to suit you. If you think you can take it on, try our advanced difficulty!"/>
        <InfoCard title={"Spectate AI"} img={spectateIcon} content="Need a breather? Watch the AI play itself. Choose which levels will be competing, and take notes!"/>
      </div>
    </div>
  )
}

type InfoCardProps = {
  title: string,
  img: string,
  content: string
}

function InfoCard({title, img, content} : InfoCardProps) {

  return(
    <div className="w-60 h-36 bg-[#303030] drop-shadow-xl rounded-sm p-4">
      <span className="flex flex-row items-center mb-3">
        <img className="mr-4 w-8 h-8" src={img} />
        <h1 className="font-bold">{title}</h1>
      </span>
      <p className='font-thin text-xs'>{content}</p>
    </div>
  )
}