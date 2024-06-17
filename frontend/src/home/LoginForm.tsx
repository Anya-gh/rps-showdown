import { useState, Dispatch, SetStateAction } from "react";
import { ChangeEvent } from "react";

type LoginFormProps = {
  setLoggedIn: Dispatch<SetStateAction<boolean>>
}

function LoginForm({ setLoggedIn } : LoginFormProps) {

  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string>();

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>, setValue: Dispatch<SetStateAction<string>>) => {
    setValue(e.target.value)
  }

  const handleSubmit = async () => {
    if (username == "") { 
      setError("Please enter your username.")
      return;
    }
    if (password == "") {
      setError("Please enter your password.")
      return;
    }
    setError(undefined)
    const request = await fetch("http://localhost:5000/access", {
      method: "POST",
      headers: { "Content-Type" : "application/json" },
      body: JSON.stringify({"Username" : username, "Password" : password})
    })
    if (request == undefined || request == null) { console.error("There was an error with the request.") }
    else if (!request.ok) {
      if (request.status == 401) { // Unauthorized
        setError("Username and password combination is incorrect. If you were trying to register, please choose a different username.")
      }
      console.error(request.statusText);
    }
    else if (request.ok) {
      const response = await request.json() as string
      console.log(response);
      localStorage.setItem("token", response);
      localStorage.setItem("username", username)
      setLoggedIn(true)
    }
    // fetch post
  }

  return (
    <div className="flex flex-col items-center my-5 w-full">
      <span className="flex flex-row items-center justify-evenly w-3/4 mb-5 md:w-1/4 text-black">
        <input onChange={(e) => {handleInputChange(e, setUsername)}} className="w-28 md:w-36 rounded-xl p-1 text-sm" placeholder="Username..." />
        <input type="password" onChange={(e) => {handleInputChange(e, setPassword)}} className="w-28 md:w-36 rounded-xl p-1 text-sm" placeholder="Password..." />
      </span>
      <button onClick={handleSubmit} className="py-1 px-3 rounded-xl bg-blue-600 text-xs mb-2 md:hover:scale-105 transition duration-200">Login / Register</button>
      <p className="italic text-xs text-center font-thin text-zinc-400 w-1/2 mb-2">Don't have an account? Enter a username and password to get started!</p>
      {error != undefined && <p className="italic text-xs text-center font-thin text-red-400 w-3/4">{error}</p>}
    </div>
  )
}

export default LoginForm;