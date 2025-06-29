namespace BeerContest.Domain.Models
{
    public static class ContestRules
    {
        public static List<ContestRule> GetDefaultContestRules()
        {
            return new List<ContestRule>
            {
                new ContestRule
                {
                    Id = "1",
                    Title = "Objeto del concurso",
                    Description = "El <b>VIII Concurso Homebrewer de Córdoba</b> con un objetivo claramente lúdico y cultural, sin ánimo de lucro.\n\nEste concurso se enmarca dentro de las actividades para el fomento de la cultura cerveza que fomenta la asociación <a href=\"https://www.facebook.com/TheRealCordobALE\" target=\"_blank\">The Real CordobALE - CerveCataClub</a>",
                    Order = 1
                },
                new ContestRule
                {
                    Id = "2",
                    Title = "Fecha y lugar de celebración",
                    Description = "23 de Noviembre de 2024 en las instalaciones de la <a href=\"https://cervezaperroflaco.com/\" target=\"_blank\">Perro Flaco &Co.</a> beer factory",
                    Order = 2
                },
                new ContestRule
                {
                    Id = "3",
                    Title = "Participación e inscripción",
                    Description = "<ul>\n<li>Podrán participar en el concurso todas aquellas personas mayores de 18 años que elaboren cerveza casera. Estas cervezas deberán ser elaboradas en equipos no comerciales en lotes menores de 100 litros. No podrán inscribirse dirección ni miembros del jurado.</li>\n<br />\n<li>El concurso se regirá en todo momento por las regulaciones y especificaciones del Beer Judge Certification Program (<a href=\"http://www.bjcp.org\" target=\"_blank\">http://www.bjcp.org</a>), en el que será inscrito como concurso oficial, y los estilos presentados se evaluarán en base a la guía de estilos publicada en 2021. (<a href=\"https://www.bjcp.org/download/2021_Guidelines_Beer.pdf\" target=\"_blank\">https://www.bjcp.org/download/2021_Guidelines_Beer.pdf</a>).</li>\n<br />\n<li>Hay dos categorías a concurso:\n<ol type=\"1\">\n<li><b>Categoría Libre: Estilos clásicos.</b> <br />Comprende los estilos clásicos de cerveza recogidos en el BJCP en las categorías 1 a la 26 (ver anexo 1)</li>\n<br />\n<li><b>Categoría propia: Historical &amp; Specialty beers</b><br />Comprende los estilos de cerveza recogidos en el BJCP en las categorías 27 a la 34 (ver anexo 2).<br />Cada concursante podrá presentar hasta 3 estilos (entradas) al concurso, siempre que las 3 entradas no pertenezcan a la misma categoría. <br />La cata de las cervezas se realizará siempre en función del estilo declarado (Tipo de cata \"Fit to style\"), pudiendo entregarse alguna mención honorífica a alguna cerveza que pese a no encontrarse dentro del estilo declarado, resalte por sus características de entre las demás.</li>\n</ol>\n</li>\n<br />\n<li>La organización se reserva el derecho de dejar fuera del concurso aquella cerveza que no cumpla con este o con cualquiera de los requisitos.</li>\n<br />\n<li>Se deberá presentar al concurso, sin marcar, ni etiquetar y sin ninguna marca que pueda revelar su procedencia, al menos 3 botellas de 33 Cl o de 50 Cl, preferentemente de vidrio o pet marrón. Se admitirán chapas o tapón cerámico de cualquier color siempre que no tengan ningún marcado. <br />Aquellos participantes que presenten menos botellas del mínimo marcado, de tamaños diferentes a los indicados, o que presenten alguna evidencia de marca o etiqueta quedarán automáticamente descalificados del concurso.<br />Si durante la recepción de las botellas, alguna sufriera daños en el transporte, se enviaría un e-mail al participante para que, en el menor plazo posible, antes de las catas de selección, se enviaran tantas botellas adicionales como botellas dañadas.</li>\n<br />\n<li>Solo se admitirán cervezas elaboradas sin extractos, es decir, deberán ser \"todo grano\". Se admitirán las adiciones de ingredientes, tales como frutas, hierbas o especias.</li>\n<br />\n<li>Para garantizar la calidad de las hojas de cata realizadas por los jueces el número máximo de cervezas admitidas a concurso será de 60, por riguroso orden de inscripción. No obstante, si el número de jueces lo permitiese, se ampliará el número de cervezas admitidas a concurso, avisando la organización con suficiente antelación.</li>\n<br />\n<li>Los interesados en participar deberán rellenar un <a href=\"https://www.concursohomebrewercordoba.es/#/registration\"> formulario de inscripción</a> por cada entrada que presenten (máximo 3). Al enviar el formulario recibirá en el correo electrónico con el que se ha inscrito un email con la etiqueta que deberá llevar adherida las botellas inscritas. Posteriormente deberá enviar el justificante de pago de la inscripción a la siguiente dirección de correo electrónico <a href=\"mailto:homebrewercordoba@gmail.com\" target=\"_blank\">homebrewercordoba@gmail.com</a>, la inscripción no se considerará completada hasta haber recibido dicho justificante de pago.</li>\n<br />\n<li>La forma de empaquetar las cervezas para su envío se deja al buen juicio del participante, pero se recomienda que las botellas se empaqueten, correctamente embaladas y separadas entre sí por cartones, en una caja adecuada, con la palabra FRÁGIL escrita en todos sus costados y flechas indicando la posición vertical. <br /></li>\n<br />\n<li>Las botellas irán identificadas unicamente con la etiqueta recibida en su email después de la inscripción, donde constará el número de entrada asignado y el estilo de la cerveza. La etiqueta debe ir sujeta a la botella mediante una goma elástica u otro método sencillo de quitar, NO DEBE IR PEGADA CON PEGAMENTOS NI SUJETA CON CINTAS ADHESIVAS, siendo esto último motivo de exclusión. <br /></li>\n<br />\n<li>La entrega de las botellas de cerveza deberá hacerse por correo o entrega física a la siguiente dirección:  <br /><br /><ol type=\"1\"><li>Hotel Casa de los Azulejos  <br />C/ Fernando Colón 5,  <br />14002 Córdoba  <br />(Indicar en el paquete: Concurso Homebrewer) <br /> <br />Los gastos de envío correrán a cargo del concursante. <br /><br />Fechas de recepción: del 10 al 17 de noviembre  <br /><br /></li></ol></li>\n<br />\n<li>No se dará por inscrito aquel participante que no complete todos los campos del formulario. En el momento en el que se reciba un envío o entrega física, los organizadores y las empresas colaboradoras, comprobarán que dicha entrega cumple con los requisitos exigidos. En caso de que no se cumplan las condiciones del apartado 1 o el producto llegue defectuoso, los promotores del concurso se pondrán en contacto con el participante para poder solucionar las incidencias observadas.</li>\n<br />\n<li>En el momento de la inscripción, la cerveza del participante recibirá un código interno por parte de la organización y los colaboradores, en base a su orden de inscripción, y que servirá para su identificación en las catas, que se realizarán siempre a ciegas para asegurar la objetividad del jurado.</li>\n<br />\n<li>Las botellas recibidas serán guardadas en un lugar seco y fresco para su conservación y serán etiquetadas con su código de inscripción, tanto en la botella como en la chapa. La organización dispondrá de un documento donde constarán los nombres de los cerveceros concursantes y el número asignado en el momento de la inscripción. Dicho documento estará resguardado por la organización y no podrá ser visualizado por terceros.</li>\n</ul>",
                    Order = 3
                },
                new ContestRule
                {
                    Id = "4",
                    Title = "Cuota de inscripción",
                    Description = "La cuota de inscripción será: <br />\n<ul>\n<li>1 cerveza 8 €</li>\n<li>2 cervezas 14 €</li>\n<li>3 cervezas 16 €</li>\n</ul>\n\n<br />\nSobre esta cuota se aplicarán los siguientes descuentos: <br /><br>\n- 2 € de descuento a quienes presenten 3 cervezas y sean socios ACCE. (hay que indicar número de socio en el formulario de inscripción). <br /><br />\n\nN° cuenta: <b>ES34 2100 4887 2622 0004 9961 </b>  (Indicar: Concurso y Nombre + Apellidos)<br /><br />\n\nEnviar justificante de pago a <a href=\"mailto:homebrewercordoba@gmail.com\">homebrewercordoba@gmail.com</a> para completar el proceso de inscripción. <br /><br />",
                    Order = 4
                },
                new ContestRule
                {
                    Id = "5",
                    Title = "Plazos",
                    Description = "<ul>\n<li>Plazo de inscripción del 1 de Septiembre al 01 de Noviembre.</li>\n<br />\n<li>Plazo de envío del 10 al 17 de Noviembre.</li>\n</ul>",
                    Order = 5
                },
                new ContestRule
                {
                    Id = "6",
                    Title = "Jurado del concurso",
                    Description = "El jurado del concurso, como norma general, estará compuesto por expertos en la elaboración y cata de cerveza, cerveceros reconocidos y Homebrewers certificados en el programa BJCP.<br />En todo caso, para cada sesión de cata del concurso se contará con al menos dos jueces BJCP que asegurarán la correcta evaluación de todas las cervezas presentadas.",
                    Order = 6
                },
                new ContestRule
                {
                    Id = "7",
                    Title = "Cata y valoración",
                    Description = "<ul>\n<li>El jurado efectuará sus valoraciones sobre una hoja de evaluación oficial, rellenando una (cada juez) por cada cerveza presentada a concurso.</li>\n<br />\n<li>Los documentos enlazados explican con claridad el detalle de los protocolos de recepción, gestión y evaluación que rigen en el programa BJCP y que se seguirán en el evento.\n<ol type=\"a\">\n<li><a href=\"https://legacy.bjcp.org/docs/SCPCompHdbk.pdf\" target=\"_blank\">https://legacy.bjcp.org/docs/SCPCompHdbk.pdf</a></li>\n<li><a href=\"https://dev.bjcp.org/competitions/competition-handbook/competition-planning-overview/\" target=\"_blank\">https://dev.bjcp.org/competitions/competition-handbook/competition-planning-overview/</a></li>\n</ol>\n</li>\n<br />\n<li>La organización realizará una única ronda eliminatoria entre todas las cervezas participantes. En dicha ronda se utilizará la Hoja de Evaluación de la Guía BJCP. Todas las cervezas participantes serán evaluadas al menos por dos jueces por lo que se devolverá a los participantes al menos dos hojas de puntuación y valoración por entrada presentada.\n\n<br /><br />\n\nEn esta ronda se desecharán aquellas cervezas que cuenten con signos evidentes de contaminación, no se encuentren dentro del estilo requerido o no cumplan con las bases del concurso.<br /><br />\n\nEl concurso puede quedar desierto, en el caso de que las cervezas participantes no cumplan con los requisitos mínimos exigidos y/o no alcancen un nivel aceptable.</li>\n<br />\n\n<li>Para optar a cualquiera de los premios del concurso, las cervezas evaluadas deberán alcanzar al menos una puntuación de 30 puntos en la escala del BJCP, pudiendo declararse desierto uno o más premios.</li>\n</ul>",
                    Order = 7
                },
                new ContestRule
                {
                    Id = "8",
                    Title = "Premios",
                    Description = "<ul>\n<li>El listado de ganadores se anunciará finalizada la cata de los jueces.</li>\n<br />\n<li>El primer clasificado de cada categoría recibirá un trofeo.</li>\n<br />\n<li>El segundo y tercer clasificado de cada categoría recibirán un diploma acreditativo.</li>\n<br />\n<li>En caso de disponer de otros premios aportados por patrocinadores para los primeros clasificados, se anunciará en redes.</li>\n</ul>",
                    Order = 8
                },
                new ContestRule
                {
                    Id = "9",
                    Title = "Rally ACCE",
                    Description = "El \"VIII Concurso Homebrewer de Córdoba\" es prueba puntuable del Rally ACCE",
                    Order = 9
                },
                new ContestRule
                {
                    Id = "10",
                    Title = "Otros datos de interés",
                    Description = "<ul>\n<li>Una vez aceptadas las bases del concurso, los finalistas tendrán derecho a conocer los resultados de su hoja de evaluación.</li>\n<br />\n<li>La organización no devolverá las botellas de cervezas que no sean utilizadas durante el concurso.</li>\n<br />\n<li>El participante se compromete y acepta que sus datos  (Nombre, Apellidos, Localidad de residencia) puedan ser divulgados en los diferentes medios de comunicación por las empresas organizadoras y colaboradoras, con objeto de dar a conocer los participantes y los premiados.</li>\n<br />\n<li>Las bases del concurso pueden ser modificadas parcial o totalmente si la organización lo estima necesario para el correcto funcionamiento del concurso; en tal caso, las personas interesadas serán debidamente informadas.</li>\n<br />\n<li>Las recetas no se harán públicas en ningún caso por parte de los organizadores, son para uso interno del jurado.</li>\n<br />\n<li>Los organizadores y patrocinadores quedan eximidos de cualquier responsabilidad en el supuesto de existir algún error en los datos facilitados en su caso por los propios agraciados que impidiera su identificación. Así mismo, los ganadores eximen a los organizadores y patrocinadores de la responsabilidad derivada de cualquier perjuicio que pudiera sufrir en el disfrute de los premios objeto del presente Concurso.</li>\n<br />\n<li>Para más información, los interesados pueden ponerse en contacto con: <a href=\"mailto:homebrewercordoba@gmail.com\">homebrewercordoba@gmail.com</a></li>\n</ul>",
                    Order = 10
                },
                new ContestRule
                {
                    Id = "11",
                    Title = "Anexo I - Categoría Libre: Estilos clásicos",
                    Description = "Comprende los estilos clásicos de cerveza recogidos en el BJCP en las categorías 1 a la 26: " +
                        GetClassicStylesDescription(),
                    Order = 11
                },
                new ContestRule
                {
                    Id = "12",
                    Title = "Anexo II - Categoría: \"Historical & Specialty beers",
                    Description = "Comprende los estilos clásicos de cerveza recogidos en el BJCP en las categorías 27 a la 34, que se detallan a continuación: " +
                        GetHistoricalStylesDescription(),
                    Order = 12
                },
                new ContestRule
                {
                    Id = "13",
                    Title = "Anexo III - Hoja de puntuación",
                    Description = "Descargar <a href=\"https://www.dropbox.com/s/3xlqcogf65vm2gj/HojaDePuntuacionV0.pdf?dl=1\" target=\"_blank\">Hoja de puntuación</a>\n<br />\nDescargar <a href=\"https://www.dropbox.com/s/3lwpbs2yp8f34y9/BJCP_HojaDePuntuacion_2018_ES%20v3.pdf?dl=1\" target=\"_blank\">Hoja de puntuación (Nueva versión)</a>`",
                    Order = 13
                }
            };
        }

        private static string GetClassicStylesDescription()
        {
            return @"<ul>
                    <li><b>1. STANDARD AMERICAN BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 1A. American Light Lager </li>
                        <li> 1B. American Lager </li>
                        <li> 1C. Cream Ale </li>
                        <li> 1D. American Wheat Beer </li>
                    </ul>
                    <br />

                    <li><b> 2. INTERNATIONAL LAGER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 2A. International Pale Lager  </li>
                        <li> 2B. International Amber Lager </li>
                        <li> 2C. International Dark Lager </li>
                    </ul>
                    <br />

                    <li><b> 3. CZECH LAGER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 3A. Czech Pale Lager </li>
                        <li> 3B. Czech Premium Pale Lager </li>
                        <li>3C. Czech Amber Lager  </li>
                        <li> 3D. Czech Dark Lager </li>
                    </ul>
                    <br />

                    <li><b> 4. PALE MALTY EUROPEAN LAGER</b></li>
                    <ul style=""list-style-type:none"">
                        <li> 4A. Munich Helles </li>
                        <li> 4B. Festbier </li>
                        <li> 4C. Helles Bock </li>
                    </ul>
                    <br />


                    <li><b>5. PALE BITTER EUROPEAN BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 5A. German Leichtbier </li>
                        <li> 5B. Kölsch </li>
                        <li> 5C. German Helles Exportbier </li>
                        <li> 5D. German Pils </li>
                    </ul>
                    <br />

                    <li><b> 6. AMBER MALTY EUROPEAN LAGER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 6A. Märzen  </li>
                        <li> 6B. Rauchbier  </li>
                        <li> 6C. Dunkles Bock </li>
                    </ul>
                    <br />

                    <li><b> 7. AMBER BITTER EUROPEAN BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 7A. Vienna Lager </li>
                        <li> 7B. Altbier </li>
                    </ul>
                    <br />

                    <li><b>8. DARK EUROPEAN LAGER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 8A. Munich Dunkel </li>
                        <li> 8B. Schwarzbier </li>
                    </ul>
                    <br />

                    <li><b> 9. STRONG EUROPEAN BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 9A. Doppelbock </li>
                        <li> 9B. Eisbock </li>
                        <li> 9C. Baltic Porter </li>
                    </ul>
                    <br />

                    <li><b>10. GERMAN WHEAT BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 10A. Weissbier  </li>
                        <li> 10B. Dunkles Weissbier </li>
                        <li> 10C. Weizenbock </li>
                    </ul>
                    <br />

                    <li><b> 11. BRITISH BITTER</b></li>
                    <ul style=""list-style-type:none"">
                        <li>  11A. Ordinary Bitter </li>
                        <li> 11B. Best Bitter </li>
                        <li> 11C. Strong Bitter </li>
                    </ul>
                    <br />

                    <li><b> 12. PALE COMMONWEALTH BE</b></li>
                    <ul style=""list-style-type:none"">
                        <li> 12A. British Golden Ale </li>
                        <li> 12B. Australian Sparkling Ale </li>
                        <li>12C. English IPA </li>
                    </ul>
                    <br />

                    <li><b> 13. BROWN BRITISH BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li>  13A. Dark Mild</li>
                        <li>13B. British Brown Ale  </li>
                        <li> 13C. English Porter </li>
                    </ul>
                    <br />

                    <li><b> 14. SCOTTISH ALE</b></li>
                    <ul style=""list-style-type:none"">
                        <li> 14A. Scottish Light  </li>
                        <li> 14B. Scottish Heavy </li>
                        <li> 14C. Scottish Export </li>
                    </ul>
                    <br />

                    <li><b> 15. IRISH BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 15A. Irish Red Ale  </li>
                        <li> 15B. Irish Stout </li>
                        <li>15C. Irish Extra Stout  </li>
                    </ul>
                    <br />

                    <li><b> 16. DARK BRITISH BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 16A. Sweet Stout </li>
                        <li> 16B. Oatmeal Stout </li>
                        <li> 16C. Tropical Stout</li>
                        <li> 16D. Foreign Extra Stout </li>
                    </ul>
                    <br />

                    <li><b> 17. STRONG BRITISH ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 17A. British Strong Ale </li>
                        <li> 17B. Old Ale </li>
                        <li> 17C. Wee Heavy </li>
                        <li> 17D. English Barley Wine </li>
                    </ul>
                    <br />

                    <li><b>18. PALE AMERICAN ALE  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 18A. Blonde Ale  </li>
                        <li>18B. American Pale Ale  </li>
                    </ul>
                    <br />

                    <li><b>19. AMBER AND BROWN AMERIC BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 19A. American Amber Ale  </li>
                        <li>19B. California Common  </li>
                        <li>19C. American Brown Ale  </li>
                    </ul>
                    <br />

                    <li><b> 20. AMERICAN PORTER AND STOUT</b></li>
                    <ul style=""list-style-type:none"">
                        <li>  20A. American Porter</li>
                        <li> 20B. American Stout </li>
                        <li> 20C. Imperial Stout </li>
                    </ul>
                    <br />

                    <li><b> 21. IPA </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 21A. American IPA  </li>
                        <li>
                            21B. Specialty IPA
                            <ul style=""list-style-type:none"">
                                <li>Specialty IPA: Belgian IPA </li>
                                <li>Specialty IPA: Black IPA </li>
                                <li>Specialty IPA: Brown IPA </li>
                                <li>Specialty IPA: Red IPA </li>
                                <li>Specialty IPA: Rye IPA </li>
                                <li>Specialty IPA: White IPA </li>
                                <li>Specialty IPA: Brut IPA </li>
                            </ul>
                        <li>21C. Hazy IPA </li>
                        </li>
                    </ul>
                    <br />

                    <li><b> 22. STRONG AMERICAN ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 22A. Double IPA  </li>
                        <li> 22B. American Strong Ale </li>
                        <li> 22C. American Barleywine </li>
                        <li> 22D. Wheatwine </li>
                    </ul>
                    <br />

                    <li><b> 23. EUROPEAN SOUR ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 23A. Berliner Weisse  </li>
                        <li> 23B. Flanders Red Ale</li>
                        <li> 23C. Oud Bruin </li>
                        <li> 23D. Lambic </li>
                        <li> 23E. Gueuze </li>
                        <li> 23F. Fruit Lambic </li>
                        <li> 23G. Gose </li>
                    </ul>
                    <br />

                    <li><b> 24. BELGIAN ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 24A. Witbier  </li>
                        <li> 24B. Belgian Pale Ale </li>
                        <li> 24C. Bière de Garde </li>
                    </ul>
                    <br />

                    <li><b> 25. STRONG BELGIAN ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 25A. Belgian Blond Ale  </li>
                        <li>25B. Saison  </li>
                        <li>25C. Belgian Golden Strong Ale  </li>
                    </ul>
                    <br />

                    <li><b> 26. TRAPPIST ALE </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 26A. Trappist Single </li>
                        <li> 26B. Belgian Dubbel </li>
                        <li> 26C. Belgian Tripel </li>
                        <li>26D. Belgian Dark Strong Ale  </li>
                    </ul>
                    <br />

                    <li><b> X. Estilos provisionales </b></li>
                    <ul style=""list-style-type:none"">
                        <li> X4. Catharina Sour </li>
                        <li> X5. Nueva Zelanda Pilsner </li>
                    </ul>
                </ul>";
        }

        private static string GetHistoricalStylesDescription()
        {
            return @"<ul>
                    <li><b>27. HISTORICAL BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 27. Historical Beer (Kellerbier)  </li>
                        <li> 27. Historical Beer (Kentucky Common) </li>
                        <li> 27. Historical Beer (Lichtenhainer) </li>
                        <li> 27. Historical Beer (London Brown Ale)  </li>
                        <li> 27. Historical Beer (Piwo Grodziskie) </li>
                        <li> 27. Historical Beer (Pre-Prohibition Lager) </li>
                        <li> 27. Historical Beer (Pre-Prohibition Porter) </li>
                        <li> 27. Historical Beer (Roggenbier) </li>
                        <li> 27. Historical Beer (Sahti) </li>
                    </ul>
                    <br />

                    <li><b>28. AMERICAN WILD ALE  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 28A. Brett Beer </li>
                        <li> 28B. Mixed-Fermentation Sour Beer </li>
                        <li> 28C. Wild Specialty Beer </li>
                        <li> 28D. Straight Sour Beer </li>
                    </ul>
                    <br />

                    <li><b> 29. FRUIT BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 29A. Fruit Beer  </li>
                        <li> 29B. Fruit and Spice Beer  </li>
                        <li> 29C. Specialty Fruit Beer </li>
                        <li> 29D. Grape Ale </li>
                    </ul>
                    <br />

                    <li><b> 30. SPICED BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 30A. Spice, Herb, or Vegetable Beer </li>
                        <li> 30B. Autumn Seasonal Beer </li>
                        <li> 30C. Winter Seasonal Beer </li>
                        <li> 30D. Specialty Spice Beer </li>
                    </ul>
                    <br />

                    <li><b>31. ALTERNATIVE FERMENTABLES BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 31A. Alternative Grain Beer  </li>
                        <li> 31B. Alternative Sugar Beer </li>
                    </ul>
                    <br />

                    <li><b>32. SMOKED BEER  </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 32A. Classic Style Smoked Beer  </li>
                        <li> 32B. Specialty Smoked Beer </li>
                    </ul>
                    <br />

                    <li><b> 33. WOOD BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 33A. Wood-Aged Beer  </li>
                        <li> 33B. Specialty Wood-Aged Beer </li>
                    </ul>
                    <br />

                    <li><b> 34. SPECIALTY BEER </b></li>
                    <ul style=""list-style-type:none"">
                        <li> 34A. Commercial Specialty Beer </li>
                        <li> 34B. Mixed-Style Beer </li>
                        <li> 34C. Experimental Beer </li>
                    </ul>
                    <br />
                </ul>";
        }
    }
}