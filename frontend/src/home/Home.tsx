import { ChangeEvent, Dispatch, SetStateAction, useState, useEffect } from "react"
import Header from "../components/Header"

/*
TODO:
- login functionality
*/

type HomeProps = {
  username: string,
  setUsername: Dispatch<SetStateAction<string>>,
  password: string,
  setPassword: Dispatch<SetStateAction<string>>
}

export default function Home({ username, setUsername, password, setPassword } : HomeProps) {

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
      <div className="h-[30rem] flex flex-col items-center justify-evenly">
        <InfoCard title={"Track your stats"} img="" content="Track your wins, losses and more! Reflect on your matches against AI, see where your weakness lie and improve."/>
        <InfoCard title={"Challenge different levels"} img="" content="Change the difficulty to suit you. If you think you can take it on, try our advanced difficulty!"/>
        <InfoCard title={"Spectate AI"} img="" content="Need a breather? Watch the AI play itself. Choose which levels will be competing, and take notes!"/>
      </div>
    </div>
  )
}

type LoginFormProps = {
  setLoggedIn: Dispatch<SetStateAction<boolean>>
}

function LoginForm({ setLoggedIn } : LoginFormProps) {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>, setValue: Dispatch<SetStateAction<string>>) => {
    setValue(e.target.value)
  }

  const handleSubmit = async () => {
    const request = await fetch("http://localhost:5000/access", {
      method: "POST",
      headers: { "Content-Type" : "application/json" },
      body: JSON.stringify({"Username" : username, "Password" : password})
    })
    if (!request.ok) {
      throw new Error(request.statusText);
    }
    else {
      const response = await request.json() as string
      console.log(response);
      localStorage.setItem("token", response);
      localStorage.setItem("username", username)
    }
    setLoggedIn(true)
    // fetch post
  }

  return (
    <div className="flex flex-col items-center my-5 w-full">
      <span className="flex flex-row items-center justify-evenly w-3/4 mb-5 text-black">
        <input onChange={(e) => {handleInputChange(e, setUsername)}} className="w-28 rounded-xl p-1 text-sm" placeholder="Username..." />
        <input onChange={(e) => {handleInputChange(e, setPassword)}} className="w-28 rounded-xl p-1 text-sm" placeholder="Password..." />
      </span>
      <button onClick={handleSubmit} className="py-1 px-3 rounded-xl bg-blue-600 text-xs mb-2">Login / Register</button>
      <p className="italic text-xs text-center font-thin text-zinc-400 w-1/2">Don't have an account? Enter a username and password to get started!</p>
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
    <div className="w-60 h-36 bg-[#303030] drop-shadow-xl rounded-sm p-2">
      <span className="flex flex-row items-center mb-3">
        <img className="mr-5 w-4 h-4 border-2 border-black" src={img} />
        <h1 className="font-bold">{title}</h1>
      </span>
      <p className='font-thin text-xs'>{content}</p>
    </div>
  )
}

type PlayOrStatsProps = {
  setLoggedIn: Dispatch<SetStateAction<boolean>>
}

function PlayOrStats({ setLoggedIn } : PlayOrStatsProps) {

  const signOutHandler = () => {
    setLoggedIn(false);
    localStorage.removeItem("token")
    localStorage.removeItem("username")
  }

  return (
    <div className="w-full flex flex-col items-center my-5">
      <div className="flex flex-row items-center w-full justify-evenly mb-5">
        <button className="w-32 h-32 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center">
          <h1 className="text-lg italic font-bold text-center mb-3">PLAY</h1>
          <img className="w-8 h-8" src="" />
        </button>
        <button className="w-32 h-32 bg-[#303030] drop-shadow-xl rounded-sm p-4 flex flex-col items-center">
          <h1 className="text-lg italic font-bold text-center mb-3">STATS</h1>
          <img className="w-8 h-8" src="" />
        </button>
      </div>
      <button onClick={signOutHandler} className="py-1 px-3 rounded-xl bg-blue-600 text-xs mb-2">Sign out</button>
    </div>
  )
}