import { ReactElement } from 'react'
import './NestedListItem.css'

export function NestedListItem({ children }: NestedListItemProps) {
  return <div className="list-item">{children}</div>
}

interface NestedListItemProps {
  children: ReactElement | ReactElement[]
}
