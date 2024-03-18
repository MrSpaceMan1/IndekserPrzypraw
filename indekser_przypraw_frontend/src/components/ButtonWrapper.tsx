import { ReactElement, MouseEvent } from 'react'

export default function ButtonWrapper({
  children,
  onClick,
}: ButtonWrapperProps) {
  return (
    <button className="unset" onClick={onClick}>
      {children}
    </button>
  )
}

interface ButtonWrapperProps {
  children: ReactElement
  onClick: (e: MouseEvent) => void | (() => void)
}
