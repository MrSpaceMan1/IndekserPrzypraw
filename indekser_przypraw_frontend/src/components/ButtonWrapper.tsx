import { ReactElement, MouseEvent, ReactNode } from 'react'
import { join } from '@/utils.ts'

export default function ButtonWrapper({
  children,
  onClick,
  additionalClasses = [],
}: ButtonWrapperProps) {
  return (
    <button
      className={join(' ', 'unset', ...additionalClasses)}
      onClick={onClick}
    >
      {children}
    </button>
  )
}

interface ButtonWrapperProps {
  children?: ReactNode | ReactNode[]
  onClick: (e: MouseEvent) => void | (() => void)
  additionalClasses?: string[]
}
