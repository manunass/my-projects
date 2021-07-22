
import React,{useState,useEffect} from 'react'
import { Col} from 'react-bootstrap';
import { MDBBtn,MDBRow } from 'mdbreact'
import axios from 'axios';
import Profile from './Profile';
import {baseUrl} from '../../package.json'

export default function Test() {
  const [props, setprops] = useState(true)
  const [recyclables, setrecyclables] = useState({Id: "", Material: "", LbpPerKg: 0})
const [current, setcurrent] = useState('')
  const [load, setload] = useState(true)
  
  useEffect( async() => {
 
   await  axios.get(baseUrl+'/api/Operations/recyclables')
      .then(
          res =>{
            console.log(res.data.Recyclables);
            setrecyclables(res.data.Recyclables);
           
            setload(false)
      }
      )
  }, []);
       
      if (load )  return  <div className="spinner-grow text-success" role="status">
      <span className="sr-only">Loading...</span>
    </div>;
    return (
        <div>
         
           <MDBRow className="row-cols-1 row-cols-sm-1 row-cols-md-1 row-cols-lg-2">
             <Col lg="2">
              <p className="h6 text-center py-4">Recyclables</p>
                    {recyclables.map((m) => (
                      <Col key={m.Id}>
                      <MDBBtn onClick={() => {
                        setcurrent(m.Id)
                        setprops(false)
                      }} size="sm" block
                      color = {current === m.Id ? "blue" :"cyan" }> {m.Material}</MDBBtn>
                      </Col>
                    ))}
              </Col>
              <Col lg="10">
                {!props ?  <Profile p={current} /> :" "}
                </Col>
          </MDBRow>
        
        </div>
    )
}
