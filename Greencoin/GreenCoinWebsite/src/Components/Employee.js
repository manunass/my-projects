import React,{useState,useEffect}from 'react'
import {Table,Form} from 'react-bootstrap'
import {MDBBtn, MDBCard, MDBCardBody, MDBCardImage } from 'mdbreact';
import axios from 'axios'
import {baseUrl} from '../../package.json'

export default function Employee() {
        const [fname, setfname] = useState('');
        const [lname, setlname] = useState('');
        const [phoneNb, setphoneNb] = useState('');
        const [email, setemail] = useState('');
        const [type, settype] = useState('')
        const [employees, setemployees] = useState([])
  
  useEffect(
    () =>{
        axios.get(baseUrl+'/api/Municipalities/0713b854-89ef-497b-a61c-e0bdb669fa70/employees').then(
          res =>{
            setemployees(res.data.Employees);
           
            console.log(res.data.Employees);
          }
        )
         
            
             
          
}, []);


        

const newEmployee = {
  FirebaseUid: "14812",
FirstName:fname,
 LastName: lname,
 PhoneNumber: phoneNb,
 Email: email,
 Type: type

};
    const handleSubmit =(e) =>{
        e.preventDefault();
        
        addEmployee();
      

    }
    const addEmployee = async() =>{
       const url =baseUrl+"/api/Municipalities/0713b854-89ef-497b-a61c-e0bdb669fa70/employees";
       await fetch(url, { method: "POST", headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(newEmployee) })
  
    
    }
    return (
        <div>
          <MDBCard className="m-1">
            <MDBCardBody>
              <Form onSubmit={handleSubmit}>
                <p className="h4 text-center py-4">Employee</p>
                <label className="grey-text font-weight-light" >First Name </label>
                <input type="text" className="form-control" value={fname} required onChange={(e) =>setfname(e.target.value)} /><br />
                <label className="grey-text font-weight-light" >Last Name </label>
                <input type="text" className="form-control" value={lname} required onChange={(e) =>setlname(e.target.value)}/><br />
                <label className="grey-text font-weight-light" >Phone Number </label>
                <input type="text" className="form-control" value={phoneNb} required onChange={(e) =>setphoneNb(e.target.value)}/><br />
                <label className="grey-text font-weight-light" >Email </label>
                <input type="email" className="form-control" value={email} required onChange={(e) =>setemail(e.target.value)}/><br />
                <br/>
                <Form.Group>
                    <Form.Label className="grey-text font-weight-light">Type</Form.Label>
                    <Form.Control as="select" onChange={(e) =>settype(e.target.value)} >
                    <option>Manager</option>
                    <option>Operator</option>
                   </Form.Control>
                </Form.Group>
                <div className="text-center py-4 mt-3">
                  <MDBBtn className="btn btn-primary" type="submit">Add</MDBBtn>
                </div>
              </Form>
            </MDBCardBody>
          </MDBCard>
       
       <br/>
        <MDBCard className="m-1">
          <MDBCardImage className='narrow-table primary-color white-text' tag='div'>
            <h2 className='h2-responsive'>Employees List </h2></MDBCardImage>
          <MDBCardBody >
          <Table>
            <thead className="head-color">
                <tr>
                <th> Name</th>
                <th>Phone Number</th>
                <th>Email</th>
                <th>Type</th>
                </tr>
            </thead>
            <tbody>
                
                 {employees.map(employee =>{
                return(  
                <tr key={employee.Id}>
                      <td>{employee.FirstName} {employee.LastName}</td>
                      <td>{employee.PhoneNumber}</td>
                      <td>{employee.Email}</td>
                      <td>{employee.Type}</td>
                 </tr>
                 ) })} 
                
            </tbody>
            </Table>
          </MDBCardBody>
        </MDBCard>
        
     
      
     
          
        </div>
    )
}
