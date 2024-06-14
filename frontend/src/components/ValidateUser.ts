import { useNavigate } from "react-router-dom"
import { useEffect } from "react"

function ValidateUser() {
  const navigate = useNavigate()

  useEffect(() => {
    const token = localStorage.getItem("token")
    if (token) { validateUser() }
    else { navigate('/') }

    async function validateUser() {
      const request = await fetch("localhost:5000/validate", {
        method: "GET",
        headers: {
          "ContentType" : "application/json",
          "Authorization" : `bearer ${token}`
        }
      })
      if (!request.ok) { navigate('/') }
    }
  })
}

export default ValidateUser;