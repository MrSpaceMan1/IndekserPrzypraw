import './App.css'
import NestedList from '@/components/NestedList.tsx'
import ButtonWrapper from '@/components/ButtonWrapper.tsx'
import hamburgerMenuSvg from '@/assets/hamburger_menu.svg'
import gearSvg from '@/assets/gear.svg'

function App() {
  return (
    <>
      <header className="header">
        <ButtonWrapper onClick={() => {}}>
          <img src={hamburgerMenuSvg} className="header-icon" />
        </ButtonWrapper>
        <h1 className="jetbrains-mono-normal no-text-wrap max-width header-title ">
          DRAWER
        </h1>
        <ButtonWrapper onClick={() => {}}>
          <img src={gearSvg} className="header-icon black-icon-filter" />
        </ButtonWrapper>
      </header>

      <NestedList listClassNames="no-list-styling">
        <NestedList header={<h2>Nested</h2>} listClassNames={'no-list-styling'}>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
          <p>2010-10-10 100g 2</p>
          <p>2010-10-10 300g 3</p>
          <p>2010-10-10 100g 42</p>
          <p>2010-10-10 200g 1</p>
        </NestedList>
      </NestedList>
    </>
  )
}

export default App
