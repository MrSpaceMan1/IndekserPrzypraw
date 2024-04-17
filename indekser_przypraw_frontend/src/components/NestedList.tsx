import { ReactElement, ReactNode, useState } from 'react'
import { Collapse } from 'react-collapse'
import ButtonWrapper from '@/components/ButtonWrapper.tsx'
import './NestedList.css'

export default function NestedList({
  children,
  label,
  headerClassNames = [],
}: NestedListParams) {
  const [isCollapsed, setIsCollapsed] = useState(!label)
  function toggleCollapse() {
    setIsCollapsed((collapsed) => !collapsed)
  }

  return (
    <div>
      {label && (
        <ButtonWrapper
          onClick={toggleCollapse}
          additionalClasses={headerClassNames}
        >
          {label}
        </ButtonWrapper>
      )}

      <Collapse isOpened={isCollapsed}>
        <div>{children}</div>
      </Collapse>
    </div>
  )
}

interface NestedListParams {
  label?: ReactElement
  openByDefault?: boolean
  children?: ReactNode[] | ReactNode | null
  listClassNames?: string[]
  headerClassNames?: string[]
}
