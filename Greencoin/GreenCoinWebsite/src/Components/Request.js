import React,{useState,useEffect} from 'react'
import {Col,Table,Row, Form, Container,Badge} from 'react-bootstrap'
import { MDBCard, MDBBtn} from
'mdbreact'
import { MDBListGroup, MDBListGroupItem } from "mdbreact";
import { FcDeleteRow ,FcApproval} from "react-icons/fc";
import axios from 'axios'
import {baseUrl} from '../../package.json'

export default function Request() {
    const [url, seturl] = useState(baseUrl+'/api/Requests?municipalityId=0713b854-89ef-497b-a61c-e0bdb669fa70')
    const [status, setstatus] = useState("")
    const [searchFilter, setSearchFilter] = useState('')
    const [load, setload] = useState(true)
    const [request, setrequest] = useState({
        Id: "",
      UserId: "",
      UnixTimeRequested: 0,
      UnixTimeCompleted: 0,
      UnixTimeApproved: 0,
      Status: ""
    })
    useEffect( () => {
    
        const getRequest = async () => {
            
            try {
               
          const reqs = await axios.get(url)
              
              setrequest(reqs.data.Requests);
              setload(false);
             
                
            } catch (err) {
              console.error(err.message);
            }
          };
        
          getRequest();
         
          
       
      }, [url,status]);
      const changeStatus= async (Status,ID,e) =>{
          console.log(Status)
          console.log("hi")
         e.preventDefault();
          if(Status==="p-a"){
        await fetch(baseUrl+'/api/Requests/'+ID+'/approve', {
            method: "PATCH", headers: { 'Content-Type': 'application/json' }})
            setstatus("Approved")
            console.log(status)
            alert("Approved")

        }
        else if (Status==="a-c"){
            await fetch(baseUrl+'/api/Requests/'+ID+'/complete', {
                method: "PATCH", headers: { 'Content-Type': 'application/json'}})
                setstatus("completed")
               console.log(status)
                alert("Completed")
        }
        else if(Status==="p-d"){
            await fetch(baseUrl+'/api/Requests/'+ID+'/decline', {
                method: "PATCH", headers: { 'Content-Type': 'application/json' }})
                setstatus("declined")
                console.log(status)
                alert("Declined")
        }
        else if (Status==="a-x"){
            await fetch(baseUrl+'/api/Requests/'+ID+'/cancel', {
                method: "PATCH", headers: { 'Content-Type': 'application/json' }})
                setstatus("cancelled")
                console.log(status)
                alert("Cancelled")
        }
        else{
            alert("..")
        }

       
    }

    return (
        <div>
            <Row>
                <Col sm="3"  md="3" lg="3">
                <MDBCard className="m-3 ">
           <p className="h7 text-center py-2">Search by Status</p>
                <Form>
                <MDBBtn onClick={(e) =>{
                    seturl(baseUrl+'/api/Requests?municipalityId=0713b854-89ef-497b-a61c-e0bdb669fa70')
                    setSearchFilter("All")
                }} size="sm" block color={searchFilter === "All" ? "cyan" : "white"}>All</MDBBtn>
 
 
                <MDBBtn  onClick={(e)=>{
                seturl(baseUrl+'/api/Requests?municipalityId=0713b854-89ef-497b-a61c-e0bdb669fa70&status=Pending')
                setSearchFilter("Pending")
                        }} size="sm" block color={searchFilter === "Pending" ? "cyan" : "white"}> Pending</MDBBtn>

                </Form>
                
              </MDBCard>
             </Col>
             <Col  sm="3" md="3" lg="5">
              <MDBCard className="m-3">
          
              {/* <p className="h7 text-center py-2">Status</p>
              <div className="p-2 ml-1">
                <Badge pill variant="info" >
                    Pending
                </Badge>{' '}
                <Badge pill variant="primary">
                    Completed
                </Badge>{' '}
                <Badge pill variant="success">
                    Approved
                </Badge>{' '}
                <Badge pill variant="danger">
                    Cancelled
                </Badge>{' '}
                <Badge pill variant="warning">
                    Declined
                </Badge>
                </div>
               */}

            
            </MDBCard>
           
            </Col>
            </Row>
            <Col sm="12" lg="12">
        <h3 align='center'><b>Requests</b></h3>

            <MDBCard  className="wrapper">
                <Table responsive>
                <thead>
                <tr>
                <th>ID</th>
                <th>User ID/Business ID</th>
                <th>UnixTimeRequested</th>
                <th>UnixTimeCompleted</th>
                <th>UnixTimeApproved</th>
                <th>Status</th>
                <th>Update</th>
                </tr>
               </thead>
               {!load ?
               <tbody>
                  
               {request.map(row =>{
                return(  
                <tr key={row.Id}>
                      <td>{row.Id}</td>
                      <td>{row.UserId == null ? row.BusinessId : row.UserId}</td>
                      <td>{row.UnixTimeRequested}</td>
                      <td>{row.UnixTimeCompleted}</td>
                      <td>{row.UnixTimeApproved}</td>
                     
                     {row.Status==="Completed"? <td><Badge pill variant="primary"  >Completed </Badge> </td>:
                      row.Status==="Cancelled"? <td><Badge pill variant="danger" >Cancelled </Badge></td>:
                      row.Status==="Declined"? <td><Badge pill variant="warning" >Declined </Badge></td>:
                      row.Status==="Pending"? <td><Badge pill variant="info" >Pending </Badge></td>:
                      row.Status==="Approved"? <td><Badge pill variant="success" >Approved </Badge> </td>:" "
                     }
                    
                    {row.Status==="Approved"? <td><MDBBtn value={row.Id} onClick={e =>changeStatus("a-c",e.target.value,e)}  color="primary" size="sm">Complete</MDBBtn>
                                                  <MDBBtn  value={row.Id} onClick={e =>changeStatus("a-x",e.target.value,e)}  color="red" size="sm">Cancel</MDBBtn> 
                                               </td>:
                      row.Status==="Pending"? <td><MDBBtn value={row.Id} onClick={e =>changeStatus("p-a",e.target.value,e)} color="light-green" size="sm">Approve</MDBBtn>
                                                  <MDBBtn value={row.Id} onClick={e =>changeStatus("p-d",e.target.value,e)}  color="yellow" size="sm">Decline</MDBBtn> 
                                                  
                                              </td>:<td></td>
               }
                 </tr>
                 ) })} 
                
               </tbody>
                : " "}
                </Table>
                </MDBCard>
           </Col>
           
         
        </div>
    )
}
