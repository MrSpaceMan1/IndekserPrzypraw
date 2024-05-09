import { ReactElement, ReactNode, useState } from 'react'
import { Collapse } from 'react-collapse'
import { ButtonWrapper } from '@/components'
import dropdownSvg from '@/assets/dropdown.svg'
import './NestedList.css'
import { join } from '@/utils.ts'

export default function NestedList({
  children,
  label,
  headerClassNames = [],
  iconClassNames = [],
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
          additionalClasses={[...headerClassNames, 'menu-header-button']}
        >
          {label}

          <img
            src={dropdownSvg}
            className={join(
              ' ',
              'menu-header-icon',
              isCollapsed ? 'menu-header-icon-rotate' : '',
              ...iconClassNames
            )}
          />
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
  iconClassNames?: string[]
}
