import { PropsWithChildren } from "react"

export default function Card(props: PropsWithChildren) {
  return (
    <div className="mb-5 bg-[#303030] drop-shadow-xl rounded-sm p-4 h-60 w-60">
      {props.children}
    </div>
  )
}
