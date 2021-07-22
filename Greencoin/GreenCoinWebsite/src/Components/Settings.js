import React,{useState} from 'react'
import { MDBCard, MDBCardBody, MDBCardImage,MDBBtn} from
'mdbreact';
import {RiCoinLine} from "react-icons/ri";
 import {FcAnswers} from "react-icons/fc"
import { Col ,Row,Form} from 'react-bootstrap';
import {baseUrl} from '../../package.json'


export default function Settings() {
  const [user, setuser] = useState(''); 
  const [newvalue, setnewvalue] = useState('')
 const data ={
  CoinsCashoutTreshold:parseInt(user)
 }
const newdata={ 
  LbpCoinRatio:parseInt(newvalue)

}
const handleSubmit = async (event) => {
event.preventDefault();
  console.log(data,user);
   await fetch(baseUrl+'/api/Municipalities/0713b854-89ef-497b-a61c-e0bdb669fa70', {
    method: "PUT", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(data)
  })
}
const handleSubmitt = async (event) => {
  event.preventDefault();
    console.log(data,newvalue);
     await fetch(baseUrl+'/api/Municipalities/0713b854-89ef-497b-a61c-e0bdb669fa70', {
      method: "PUT", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(newdata)
    })
  }
  
 
    return (
        <div>
            <Row className="row-cols-1 row-cols-sm-2 row-cols-md-2"> 
            <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage className='narrow winter-neva-gradient ' tag="div">
                    <FcAnswers/>
                    </MDBCardImage>
                    <MDBCardBody  className='text-center'>
                    <Form onSubmit={handleSubmit}>  
                    
                    <label className="grey-text font-weight-light" >Coins Cashout Treshold </label>
                    <input type="text" className="form-control"  value={user} required onChange={(e) =>setuser(e.target.value)}/>
                    <div className="text-center  mt-1">
                      <MDBBtn color="success" type="submit">Edit</MDBBtn>
                    </div>
                    </Form>
                    </MDBCardBody>
                    </MDBCard>
                   </Col>

                   
                   <Col>
                    <MDBCard className="wrapper">
                    <MDBCardImage className='narrow winter-neva-gradient ' tag='div' >
                        <RiCoinLine/>
                    </MDBCardImage>
                    <MDBCardBody  className='text-center'>
                       
                    <Form onSubmit={handleSubmitt}> 
                    
                        <label className="grey-text font-weight-light" >LBP To Coins Ratio</label>
                        <input type="text" className="form-control" value={newvalue} required  onChange={(e) =>setnewvalue(e.target.value)} />
                        <div className="text-center  mt-1">
                        <MDBBtn color="success" type="submit">Edit</MDBBtn>
                        </div>
                    </Form>
                    </MDBCardBody>
                    </MDBCard>
                    </Col>
            </Row>
        </div>
    )
    }
