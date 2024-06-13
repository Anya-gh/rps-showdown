import { ChangeEvent, Dispatch, SetStateAction, useState } from "react"

export default function Home() {
  return (
    <div className="flex flex-col items-center px-5 py-20 w-screen">
      <span className="text-center">
        <h1 className="font-bold text-4xl">RPS Showdown</h1>
        <p className="italic text-xs font-thin">Return to the age old classic!</p>
      </span>
      <LoginForm />
      <div className="h-[30rem] flex flex-col items-center justify-evenly">
        <InfoCard title={"Track your stats"} img="" content="Track your wins, losses and more! Reflect on your matches against AI, see where your weakness lie and improve."/>
        <InfoCard title={"Challenge different levels"} img="" content="Change the difficulty to suit you. If you think you can take it on, try our advanced difficulty!"/>
        <InfoCard title={"Spectate AI"} img="" content="Need a breather? Watch the AI play itself. Choose which levels will be competing, and take notes!"/>
      </div>
    </div>
  )
}

function LoginForm() {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>, setValue: Dispatch<SetStateAction<string>>) => {
    setValue(e.target.value)
  }

  const handleSubmit = () => {
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

type InfoCard = {
  title: string,
  img: string,
  content: string
}

function InfoCard({title, img, content} : InfoCard) {
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
