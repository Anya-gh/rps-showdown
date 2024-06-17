import Modal from "react-modal"
import { Dispatch, SetStateAction } from "react"

type ConfirmChangeProps = {
  modalOpen: boolean,
  setModalOpen: Dispatch<SetStateAction<boolean>>,
  chosenLevel: number | undefined,
  chosenPlayer: number,
  handleCancel: () => void,
  handleStart: (chosenLevel: (number | undefined), chosenPlayer: number) => void
}

function ConfirmChange({ modalOpen, setModalOpen, chosenLevel, chosenPlayer, handleCancel, handleStart} : ConfirmChangeProps) {

  return (
    <Modal
      isOpen={modalOpen}
      onRequestClose={() => setModalOpen(false)}
      className="w-screen h-screen flex flex-col items-center justify-center bg-black opacity-70 z-20"
    >
      <div>
        <div className="w-60 bg-[#303030] text-center rounded-xl p-3">
          <p className="text-sm mb-3">Are you sure you want to change who's playing? Any bots will be reset and will not be able to see any past moves thus far.</p>
          <div className="flex flex-row items-center justify-evenly">
            <button onClick={() => handleStart(chosenLevel, chosenPlayer)} className="py-1 px-3 bg-blue-500 rounded-lg">Yes</button>
            <button onClick={handleCancel} className="p-1 px-3 bg-red-500 rounded-lg">No</button>
          </div>
        </div>
      </div>
    </Modal>
  )
}

export default ConfirmChange;