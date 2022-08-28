import './App.css';
import './layouts/MainLayout'
import Home from './pages/Home';
import MainLayout from './layouts/MainLayout';
import React from 'react'
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom'

function App() {
  return (
    <Router>
      <MainLayout>
        <Routes>
          <Route path='/' element={<Home></Home>}/>
        </Routes>
      </MainLayout>
    </Router>
  );
}

export default App;
