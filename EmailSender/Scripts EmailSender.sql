USE MORE
go
select * from SyMailTemplate
--insertar los nuevos template para correr en otros ambientes
go
insert into SyMailTemplate (TemplateID,Portal, TemplateName , TemplateText, SubjectText, MailFrom, Voided, Observaciones, Adduser, Adddate) 
values (12,'CON', 'More - Overdue days','<html>

<head>
    <title>Page Title</title>
    <style>
        html,
        body {
            margin: 0 auto !important;
            padding: 0 !important;
            height: 100% !important;
            width: 100% !important;
            font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; 
            font-size:12pt;
        }

        * {
            -ms-text-size-adjust: 100%;
        }

        .invoice{
            mso-table-lspace: 0pt !important;
            mso-table-rspace: 0pt !important;
            padding: 10px;
            border: 2px solid #E2E2E2;
            text-align: center;
        }

        img {
            -ms-interpolation-mode: bicubic;
        }

        a {
            text-decoration: none;
        }
    </style>
</head>

<body width="100%" style="margin: 0; padding: 0 !important; mso-line-height-rule: exactly;">
    <div style="margin-top: 10px; padding: 20px;">
        <p> Dear {xx_CLIENT_NAME}, </p>
        <p> I hope this email finds you well. </p>
        <p> According to our records, the invoices listed below have not been settled, and are now overdue. </p>
        <table class="invoice" style="width: 95%; border-collapse: collapse; margin: 0; border: 2px solid #E2E2E2; border-radius: 5px; color: #646464; background: white; font-weight: normal;">
            <tr>
                <td class="invoice" style=" font-size:20px; font-weight:bold; text-align:center;"> INVOICE NUMBER </td>
                <td class="invoice" style=" font-size:20px; font-weight:bold; text-align:center;"> OUR REF. </td>
                <td class="invoice" style=" font-size:20px; font-weight:bold; text-align:center;"> INVOICE DATE </td>
                <td class="invoice" style=" font-size:20px; font-weight:bold; text-align:center;"> DAYS PAST-DUE DATE </td>
                <td class="invoice" style=" font-size:20px; font-weight:bold; text-align:center;"> INVOICE AMOUNT </td>
            </tr>
            {xx_Inovoices_rows}
            <tr>
                <td colspan="3"></td>
                <td class="invoice">Total</td>
                <td class="invoice">{xx_TOTAL}</td>
            </tr>
        </table> 
        <em style="font-size: 15px;"> *Please notice that there might exist other invoices that are not due yet. </em>
        <br />
        <p> We have done our best and professional work managing your case at the most reasonable price. </p>
        <p> We understand that all our invoices were correctly accepted by you as we have not received any rejection notice and we have handled your cases with responsibility and professionalism. </p>
        <p> Please notice that our fees were calculated with the assumption that our invoice were going to be cancelled on due date. We ask you, please, to cancel our invoices in the next business days. </p>
        <p> Thank you for understanding and always answering promptly. </p>
        <p> Kind regards, </p>

    </div>
    <table  style="margin:0px; table-layout:fixed; width:0px; border-collapse:collapse; color:rgb(0,0,0); font-family:Segoe UI,Segoe UI Web,Arial,Verdana,sans-serif; font-size:12px; text-align:start; background:transparent; border-spacing:0px">
        <tbody style="margin:0px">
            <tr style="margin:0px; height:93px">
                <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                    <div style="margin:0px; padding:0px 7px">
                        <div style="margin:0px; clear:both">
                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:20.5042px; font-family:Calibri,Calibri_EmbeddedFont,Calibri_MSFontService,sans-serif"/>
                                <span style="margin:0px; left:0px; top:2px; display:inline-block; width:auto; height:auto">
                                <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:11pt; line-height:17.2667px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif"/>
                                <span style="margin:0px; font-size:11pt; line-height:17.2667px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp; </span>
                            </p>
                            <table style="margin:0px; table-layout:fixed; width:0px; border-collapse:collapse; font-family: Segoe UI,Segoe UI Web,Arial,Verdana,sans-serif; font-size:12px; text-align:start; background:transparent; border-spacing:0px">
                                <tbody style="margin:0px">
                                    <tr style="margin:0px; height:93px">
                                        <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                            <div style="margin:0px; padding:0px 7px">
                                                <div style="margin:0px; clear:both">
                                                    <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <img src="http://www.moellerip-more.com/Images/More/MIP_Logo_RGB_6000px_9.png" width="176px"  height="45px" style="width:176px; height:45px;"/>
                                                        </p>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="margin:0px; height:54px">
                                            <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                <div style="margin:0px; padding:0px 7px">
                                                    <div style="margin:0px; clear:both">
                                                        <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; font-weight:bold">
                                                                <span style="margin:0px">Financial Department </span>
                                                            </span>
                                                            <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                        </p>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="margin:0px; height:37px">
                                            <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                <div style="margin:0px; padding:0px 7px">
                                                    <div style="margin:0px; clear:both">
                                                        <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <span style="margin:0px; left:-4px; top:8px; display:inline-block; z-index:251685888; width:auto; height:auto">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px"/>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style="margin:0px; height:97px">
                                                <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                    <div style="margin:0px; padding:0px 7px">
                                                        <div style="margin:0px; clear:both">
                                                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; font-weight:bold">
                                                                    <span style="margin:0px">Moeller IP Advisors</span>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                            </p>
                                                        </div>
                                                        <div style="margin:0px; clear:both">
                                                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px">Covered Regions: Americas</span>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:WordVisiCarriageReturn_MSFontService,Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px">&nbsp;</span>
                                                                    <br style="margin:0px"/>
                                                                    <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Calibri,Calibri_EmbeddedFont,Calibri_MSFontService,sans-serif"/>
                                                                    <a href="https://www.moellerip.com/" target="_blank" rel="noreferrer noopener" data-auth="NotApplicable" style="margin:0px" data-linkindex="0">
                                                                        <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; color:rgb(103,165,174); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                            <span style="margin:0px">moellerip.com</span>
                                                                        </span>
                                                                    </a>
                                                                    <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif"/>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr style="margin:0px; height:96px">
                                                    <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                        <div style="margin:0px; padding:0px 7px">
                                                            <div style="margin:0px; clear:both">
                                                                <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                    <span lang="EN-US" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">This communication is confidential and may be protected by professional confidentialities and should therefore be reviewed only by the addressee.</span>
                                                                    </span>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; color:rgb(128,128,128)">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                            <div style="margin:0px; clear:both">
                                                                <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                    <span lang="EN-US" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">If you have received it by mistake, please return it to the sender, delete it from your system and do not disclose its contents to anyone.<span>&nbsp;</span>
                                                                        </span>
                                                                    </span>
                                                                    <span lang="DE-DE" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">Thank</span>
                                                                        <span style="margin:0px">
                                                                            <span>&nbsp;</span>
                                                                        </span>
                                                                        <span style="margin:0px">you</span>
                                                                        <span style="margin:0px">.</span>
                                                                    </span>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; color:rgb(128,128,128)">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <p>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
</body>

</html>','Overdue Invoices - Moeller IP (RESPONSE REQUIRED)','invoices@moellerip.com','N','el email informa que se vencio su pago', 20189,GETDATE())
go
insert into SyMailTemplate (TemplateID,Portal, TemplateName , TemplateText, SubjectText, MailFrom, Voided, Observaciones, Adduser, Adddate) 
values (13,'CON', 'More - Reminder','<html>

<head>
    <title>Page Title</title>
    <style>
        html,
        body {
            margin: 0 auto !important;
            padding: 0 !important;
            height: 100% !important;
            width: 100% !important;
            font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; 
            font-size:12pt;
        }

        * {
            -ms-text-size-adjust: 100%;
        }

        img {
            -ms-interpolation-mode: bicubic;
        }

        a {
            text-decoration: none;
        }
    </style>
</head>

<body width="100%" style="margin: 0; padding: 0 !important; mso-line-height-rule: exactly;">
    <div style="margin-top: 10px; padding: 20px;">
        <p> Dear {xx_CLIENT_NAME}, </p>
        <p> I hope this email finds you well. </p>
        <p> We just want to let you know that the invoice {xx_INVOICE#} amunting to {xx_INVOICE_AMOUNT} is near to due.  </p>
        <p> We understand that the invoice was correctly accepted by you as we have not received any rejection notice and we have handled your cases with responsibility and professionalism. </p>
        <p> Should you not be the responsible person for this matter, we kindly ask you to forward this email to the corresponding person and/or department in your firm, copying us, so we can update our database and avoid sending you this sort of emails in the future.  </p>
        <p> If you have paid the abovementioned invoice during the last week, please disregard this automatic email. </p>
        <p> Thank you for time. </p>
        <p> Kind regards, </p>
    </div>
    <table style="margin:0px; table-layout:fixed; width:0px; border-collapse:collapse; color:rgb(0,0,0); font-family:Segoe UI,Segoe UI Web,Arial,Verdana,sans-serif; font-size:12px; text-align:start; background:transparent; border-spacing:0px">
        <tbody style="margin:0px">
            <tr style="margin:0px; height:93px">
                <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                    <div style="margin:0px; padding:0px 7px">
                        <div style="margin:0px; clear:both">
                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:20.5042px; font-family:Calibri,Calibri_EmbeddedFont,Calibri_MSFontService,sans-serif"/>
                                <span style="margin:0px; left:0px; top:2px; display:inline-block; width:auto; height:auto">
                                <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:11pt; line-height:17.2667px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif"/>
                                <span style="margin:0px; font-size:11pt; line-height:17.2667px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp; </span>
                            </p>
                            <table style="margin:0px; table-layout:fixed; width:0px; border-collapse:collapse; font-family: Segoe UI,Segoe UI Web,Arial,Verdana,sans-serif; font-size:12px; text-align:start; background:transparent; border-spacing:0px">
                                <tbody style="margin:0px">
                                    <tr style="margin:0px; height:93px">
                                        <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                            <div style="margin:0px; padding:0px 7px">
                                                <div style="margin:0px; clear:both">
                                                    <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <img src="http://www.moellerip-more.com/Images/More/MIP_Logo_RGB_6000px_9.png" width="176px"  height="45px" style="width:176px; height:45px;"/>
                                                        </p>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="margin:0px; height:54px">
                                            <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                <div style="margin:0px; padding:0px 7px">
                                                    <div style="margin:0px; clear:both">
                                                        <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; font-weight:bold">
                                                                <span style="margin:0px">Financial Department </span>
                                                            </span>
                                                            <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                        </p>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr style="margin:0px; height:37px">
                                            <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                <div style="margin:0px; padding:0px 7px">
                                                    <div style="margin:0px; clear:both">
                                                        <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                            <span style="margin:0px; left:-4px; top:8px; display:inline-block; z-index:251685888; width:auto; height:auto">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px"/>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                            </p>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr style="margin:0px; height:97px">
                                                <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                    <div style="margin:0px; padding:0px 7px">
                                                        <div style="margin:0px; clear:both">
                                                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; font-weight:bold">
                                                                    <span style="margin:0px">Moeller IP Advisors</span>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                            </p>
                                                        </div>
                                                        <div style="margin:0px; clear:both">
                                                            <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px">Covered Regions: Americas</span>
                                                                </span>
                                                                <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:WordVisiCarriageReturn_MSFontService,Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                    <span style="margin:0px">&nbsp;</span>
                                                                    <br style="margin:0px"/>
                                                                    <span lang="DE-DE" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Calibri,Calibri_EmbeddedFont,Calibri_MSFontService,sans-serif"/>
                                                                    <a href="https://www.moellerip.com/" target="_blank" rel="noreferrer noopener" data-auth="NotApplicable" style="margin:0px" data-linkindex="0">
                                                                        <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; color:rgb(103,165,174); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                            <span style="margin:0px">moellerip.com</span>
                                                                        </span>
                                                                    </a>
                                                                    <span lang="EN-US" style="margin:0px; font-variant-ligatures:none!important; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif"/>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr style="margin:0px; height:96px">
                                                    <td style="margin:0px; vertical-align:top; border-width:0px; border-style:none; width:666px">
                                                        <div style="margin:0px; padding:0px 7px">
                                                            <div style="margin:0px; clear:both">
                                                                <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                    <span lang="EN-US" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">This communication is confidential and may be protected by professional confidentialities and should therefore be reviewed only by the addressee.</span>
                                                                    </span>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; color:rgb(128,128,128)">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                            <div style="margin:0px; clear:both">
                                                                <p style="margin:0px; font-weight:normal; font-kerning:none; color:windowtext; text-align:left">
                                                                    <span lang="EN-US" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">If you have received it by mistake, please return it to the sender, delete it from your system and do not disclose its contents to anyone.<span>&nbsp;</span>
                                                                        </span>
                                                                    </span>
                                                                    <span lang="DE-DE" style="margin:0px; outline:transparent solid 1px; font-variant-ligatures:none!important; color:rgb(128,128,128); background-color:rgb(255,255,255); font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif">
                                                                        <span style="margin:0px">Thank</span>
                                                                        <span style="margin:0px">
                                                                            <span>&nbsp;</span>
                                                                        </span>
                                                                        <span style="margin:0px">you</span>
                                                                        <span style="margin:0px">.</span>
                                                                    </span>
                                                                    <span style="margin:0px; font-size:12pt; line-height:21.6px; font-family:Arial,Arial_EmbeddedFont,Arial_MSFontService,sans-serif; color:rgb(128,128,128)">&nbsp;</span>
                                                                </p>
                                                            </div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                        <p>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
</body>

</html>','More - Reminder','invoices@moellerip.com','N','el email informa que esta a 7 dias de vencer su pago', 20189,GETDATE())
go

--------------------------------------------

USE [Patricia_MoellerIp]
GO
CREATE TABLE [dbo].[INVOICE_OVERDUE_EMAIL_LOG](
	[INVOICE_OVERDUE_LOG_ID] [int] IDENTITY(1,1) NOT NULL,
	[CLIENT_NAME] [nvarchar](150) NOT NULL,
	[ACTION_DATE] [datetime] NOT NULL,
 CONSTRAINT [PK_INVOICE_OVERDUE_EMAIL_LOG] PRIMARY KEY CLUSTERED 
(
	[INVOICE_OVERDUE_LOG_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

create or alter procedure sp_moe_0011_Reminder_EmailTemplate
as begin

select * from MORE.dbo.SyMailTemplate where TemplateID = 13

end

------------------------------------------------
GO
create or alter procedure sp_moe_0012_Overdue_Days_EmailTemplate
as begin

select * from MORE.dbo.SyMailTemplate where TemplateID = 12
end

go
CREATE OR ALTER PROCEDURE RegisterInvoiceOverdueLog
@Client_Name nvarchar(150)
as begin
insert into INVOICE_OVERDUE_EMAIL_LOG values (@Client_Name,GETDATE())
end
--------------------------------------------------
go
--nueva funcion para obtener el mail de las personas que tienen dias vencidos

CREATE FUNCTION [dbo].[gf_moe_GetInvoiceEmailsFromCase] (@CASE_ID int ) RETURNS NVARCHAR(MAX)
AS
BEGIN


DECLARE @RESULT AS NVARCHAR(MAX)
DECLARE @TMP_EMAILS TABLE (EMAIL varchar(MAX))
DECLARE @ROLE_TYPE INT

--seteo el rol por si uno no existe
IF EXISTS(SELECT ACTOR_ID FROM CASTING WHERE CASE_ID = @CASE_ID AND ROLE_TYPE_ID IN (20007))
	BEGIN
		SET @ROLE_TYPE = 20007
	END
ELSE
	BEGIN
		SET @ROLE_TYPE = 4
	END


INSERT INTO @TMP_EMAILS (EMAIL)
SELECT LOWER(LTRIM(RTRIM(E_MAIL_ADRESS)))
FROM Patricia_Moellerip.dbo.PAT_NAMES_ADDRESS
WHERE E_MAIL_ADRESS IS NOT NULL AND NAME_ID IN
(SELECT ACTOR_ID FROM CASTING WHERE CASE_ID = @CASE_ID AND ROLE_TYPE_ID IN (@ROLE_TYPE)) AND CURRENT_ONE = 1 
AND NOT EXISTS (SELECT EMAIL FROM @TMP_EMAILS WHERE EMAIL = LOWER(LTRIM(RTRIM(E_MAIL_ADRESS))))




SELECT @RESULT = COALESCE(@RESULT + ', ', '') + EMAIL
FROM @TMP_EMAILS
ORDER BY EMAIL



RETURN @result



END
go
create or alter procedure sp_moe_GetMails_Overdue_Days
as begin

SELECT CAST (dbo.fn_case_number(O.CASE_ID) AS NVARCHAR(20)) AS 'CASE_NUMBER',
CAST (dbo.fn_moe_GetAccountName(O.CASE_ID) AS NVARCHAR(80)) AS 'ACCOUNT_NAME',
CAST(O.CREDIT_TIME AS NVARCHAR(20)) AS 'CREDIT_TIME',
O.OVERDUE_DAYS AS 'OVERDUE_DAYS', --PAST-DUE DATE
dbo.gf_moe_GetInvoiceEmailsFromCase(O.CASE_ID) as EmailSender,
O.INVOICE_PATRICIA AS 'INVOICE_PATRICIA',
O.INVOICE_TANGO AS 'INVOICE_TANGO', --este es INVOICE #
dbo.fn_moe_GetCaseReference(O.CASE_ID) as 'YOUR_REF' ,
O.INVOICE_DATE AS 'INVOICE_DATE',
CAST(H.FOREIGN_CURR_ID AS NVARCHAR(20)) AS 'CURRENCY',
H.FOREIGN_CURR_VALUE AS 'INVOICE_AMOUNT'
FROM MOE_OVERDUE_INVOICES_LOG O INNER JOIN INVOICE_HEADER H ON O.INVOICE_PATRICIA = H.INVOICE_ID
WHERE O.INVOICE_PAID IS NULL AND IS_BILLTRADER = 0 and (OVERDUE_DAYS =-7 or OVERDUE_DAYS >=0)
AND dbo.gf_moe_GetServiceLevel(O.CASE_ID) NOT IN (20003, 20004, 20009, 20010, 20011) AND dbo.fn_moe_GetAccountNameID(O.CASE_ID) <> 1751
ORDER BY ACCOUNT_NAME, INVOICE_DATE

end
--------------------------------------------
